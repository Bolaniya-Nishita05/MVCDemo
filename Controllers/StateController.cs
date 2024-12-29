using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using MVCDemo.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MVCDemo.Controllers
{
    public class StateController : Controller
    {
        private readonly IConfiguration _configuration;

        #region configuration
        public StateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Index
        public IActionResult Index(int? CountryID)
        {
            string connectionstr = this._configuration.GetConnectionString("ConnectionString");
            //PrePare a connection
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            //Prepare a Command
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_State_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();
            DataTable table1 = new DataTable();
            table1.Load(objSDR);

            SqlCommand objCmd2 = conn.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_LOC_State_StateWiseCityCount";
            SqlDataReader objSDR2 = objCmd2.ExecuteReader();
            DataTable table2 = new DataTable();
            table2.Load(objSDR2);

            table1.Columns.Add("CityCount", typeof(int));

            for (int i = 0; i < table1.Rows.Count; i++)
            {
                table1.Rows[i]["CityCount"] = table2.Rows[i]["CityCount"];
            }


            if (CountryID != null)
            {
                DataTable filteredTable = new DataTable();

                foreach (DataColumn column in table1.Columns)
                {
                    filteredTable.Columns.Add(column.ColumnName, column.DataType); ;
                }

                foreach (DataRow dr in table1.Rows)
                {
                    if (CountryID == Convert.ToInt32(@dr["CountryID"]))
                    {
                        DataRow newDataRow = filteredTable.NewRow();

                        foreach (DataColumn column in table1.Columns)
                        {
                            newDataRow[column.ColumnName] = @dr[column.ColumnName];
                        }

                        filteredTable.Rows.Add(newDataRow);
                    }
                }

                conn.Close();
                return View("Index", filteredTable);
            }

            conn.Close();
            return View("Index", table1);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int StateID)
        {
            try
            {
                string connectionstr = _configuration.GetConnectionString("ConnectionString");
                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    using (SqlCommand sqlCommand = conn.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "PR_LOC_State_Delete";
                        sqlCommand.Parameters.AddWithValue("@StateID", StateID);
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                TempData["ErrorMsg"] = "Deletion of record failed";
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region StateAddEdit
        [Route("State/StateAddEdit")]
        public IActionResult StateAddEdit(int? StateID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_LOC_Country_SelectComboBox";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<CountryDropDownModel> countryList = new List<CountryDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                CountryDropDownModel countryDropDownModel = new CountryDropDownModel();
                countryDropDownModel.CountryID = Convert.ToInt32(data["CountryID"]);
                countryDropDownModel.CountryName = data["CountryName"].ToString();
                countryList.Add(countryDropDownModel);
            }
            ViewBag.CountryList = countryList;

            if (StateID == null)
            {
                return View(new StateModel());
            }

            #region StateByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_State_SelectByPK";
            command.Parameters.AddWithValue("@StateID", StateID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            StateModel stateModel = new StateModel();

            foreach (DataRow dataRow in table.Rows)
            {
                stateModel.StateID = Convert.ToInt32(@dataRow["StateID"]);
                stateModel.StateName = @dataRow["StateName"].ToString();
                stateModel.CountryID = Convert.ToInt32(@dataRow["CountryID"]);
                stateModel.StateCode = @dataRow["StateCode"].ToString();
            }

            #endregion

            return View(stateModel);
        }

        public IActionResult onSubmit(StateModel stateModel)
        {
            if (stateModel.CountryID <= 0)
            {
                ModelState.AddModelError("Country", "A valid Country is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (stateModel.StateID == 0)
                {
                    command.CommandText = "PR_LOC_State_Insert";
                }
                else
                {
                    command.CommandText = "PR_LOC_State_Update";
                    command.Parameters.Add("@StateID", SqlDbType.Int).Value = stateModel.StateID;
                }
                command.Parameters.Add("@StateName", SqlDbType.VarChar).Value = stateModel.StateName;
                command.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = stateModel.StateCode;
                command.Parameters.Add("@CountryID", SqlDbType.Int).Value = stateModel.CountryID;
                command.ExecuteNonQuery();

                if (stateModel.StateID == 0)
                {
                    TempData["StateInsertMsg"] = "Record inserted successfully";
                }
                else
                {
                    TempData["StateUpdateMsg"] = "Record updated successfully";
                }

                return RedirectToAction("Index");
            }

            return View("StateAddEdit", stateModel);
        }

        #endregion
    }

}

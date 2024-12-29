using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using MVCDemo.Models;

namespace MVCDemo.Controllers
{
    public class CountryController : Controller
    {
        private readonly IConfiguration _configuration;

        #region configuration
        public CountryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            string connectionstr = this._configuration.GetConnectionString("ConnectionString");
            //PrePare a connection
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            //Prepare a Command
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_Country_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();
            DataTable table1 = new DataTable();
            table1.Load(objSDR);

            SqlCommand objCmd2 = conn.CreateCommand();
            objCmd2.CommandType = CommandType.StoredProcedure;
            objCmd2.CommandText = "PR_LOC_Country_CountryWiseStateCount";
            SqlDataReader objSDR2 = objCmd2.ExecuteReader();
            DataTable table2 = new DataTable();
            table2.Load(objSDR2);

            table1.Columns.Add("StateCount", typeof(int));

            for (int i = 0; i < table1.Rows.Count; i++)
            {
                table1.Rows[i]["StateCount"] = table2.Rows[i]["StateCount"];
            }


            conn.Close();
            return View("Index", table1);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CountryID)
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
                        sqlCommand.CommandText = "PR_LOC_Country_Delete";
                        sqlCommand.Parameters.AddWithValue("@CountryID", CountryID);
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

        #region CountryAddEdit
        [Route("Country/CountryAddEdit")]
        public IActionResult CountryAddEdit(int? CountryID)
        {
            if (CountryID == null)
            {
                return View(new CountryModel());
            }

            #region CountryByID

            string connectionString = _configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_Country_SelectByPK";
            command.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            CountryModel countryModel = new CountryModel();

            foreach (DataRow dataRow in table.Rows)
            {
                countryModel.CountryID = Convert.ToInt32(@dataRow["CountryID"]);
                countryModel.CountryName = @dataRow["CountryName"].ToString();
                countryModel.CountryCode = @dataRow["CountryCode"].ToString();
            }

            #endregion

            return View(countryModel);
        }

        public IActionResult onSubmit(CountryModel countryModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (countryModel.CountryID == 0)
                {
                    command.CommandText = "PR_LOC_Country_Insert";
                }
                else
                {
                    command.CommandText = "PR_LOC_Country_Update";
                    command.Parameters.Add("@CountryID", SqlDbType.Int).Value = countryModel.CountryID;
                }
                command.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = countryModel.CountryName;
                command.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = countryModel.CountryCode;
                command.ExecuteNonQuery();

                if (countryModel.CountryID == 0)
                {
                    TempData["CountryInsertMsg"] = "Record inserted successfully";
                }
                else
                {
                    TempData["CountryUpdateMsg"] = "Record updated successfully";
                }

                return RedirectToAction("Index");
            }

            return View("CountryAddEdit", countryModel);
        }

        #endregion
    }

}

using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using MVCDemo.Models;

namespace MVCDemo.Controllers
{
    public class CityController : Controller
    {
        private readonly IConfiguration _configuration;

        #region configuration
        public CityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Index
        public IActionResult Index(int? StateID)
        {
            string connectionstr = this._configuration.GetConnectionString("ConnectionString");
            //PrePare a connection
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstr);
            conn.Open();

            //Prepare a Command
            SqlCommand objCmd = conn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_LOC_City_SelectAll";

            SqlDataReader objSDR = objCmd.ExecuteReader();
            dt.Load(objSDR);

            if (StateID != null)
            {
                DataTable filteredTable = new DataTable();

                foreach (DataColumn column in dt.Columns)
                {
                    filteredTable.Columns.Add(column.ColumnName, column.DataType);;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    if (StateID==Convert.ToInt32(@dr["StateID"]))
                    {
                        DataRow newDataRow = filteredTable.NewRow();

                        foreach (DataColumn column in dt.Columns)
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
            return View("Index", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(string CityID)
        {
            // Decrypt the CityID
            int decryptedCityID = Convert.ToInt32(UrlEncryptor.Decrypt(CityID.ToString()));


            try
            {
                string connectionstr = _configuration.GetConnectionString("ConnectionString");
                using (SqlConnection conn = new SqlConnection(connectionstr))
                {
                    conn.Open();
                    using (SqlCommand sqlCommand = conn.CreateCommand())
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "PR_LOC_City_Delete";
                        sqlCommand.Parameters.AddWithValue("@CityID", decryptedCityID);
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

        #region GetStatesByCountry
        // AJAX handler for loading states dynamically
        [HttpPost]
        public JsonResult GetStatesByCountry(int CountryID)
        {
            List<StateDropDownModel> loc_State = GetStateByCountryID(CountryID); // Fetch states
            return Json(loc_State); // Return JSON response
        }
        #endregion

        #region GetStateByCountryID
        // Helper method to fetch states by country ID
        public List<StateDropDownModel> GetStateByCountryID(int CountryID)
        {
            string connectionstr = _configuration.GetConnectionString("ConnectionString");
            List<StateDropDownModel> loc_State = new List<StateDropDownModel>();

            using (SqlConnection conn = new SqlConnection(connectionstr))
            {
                conn.Open();
                using (SqlCommand objCmd = conn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_LOC_State_SelectComboBoxByCountryID";
                    objCmd.Parameters.AddWithValue("@CountryID", CountryID);

                    using (SqlDataReader objSDR = objCmd.ExecuteReader())
                    {
                        if (objSDR.HasRows)
                        {
                            while (objSDR.Read())
                            {
                                loc_State.Add(new StateDropDownModel
                                {
                                    StateID = Convert.ToInt32(objSDR["StateID"]),
                                    StateName = objSDR["StateName"].ToString()
                                });
                            }
                        }
                    }
                }
            }

            return loc_State;
        }
        #endregion


        #region CityAddEdit
        [Route("City/CityAddEdit")]
        public IActionResult CityAddEdit(string? CityID)
        {
            int? decryptedCityID = null;

            // Decrypt only if CityID is not null or empty
            if (!string.IsNullOrEmpty(CityID))
            {
                string decryptedCityIDString = UrlEncryptor.Decrypt(CityID); // Decrypt the encrypted CityID
                decryptedCityID = int.Parse(decryptedCityIDString); // Convert decrypted string to integer
            }


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

            if (string.IsNullOrEmpty(CityID))
            {
                return View(new CityModel());
            }

            #region CityByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_LOC_City_SelectByPK";
            command.Parameters.AddWithValue("@CityID", decryptedCityID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            CityModel cityModel = new CityModel();

            foreach (DataRow dataRow in table.Rows)
            {
                cityModel.CityID = Convert.ToInt32(@dataRow["CityID"]);
                cityModel.CityName = @dataRow["CityName"].ToString();
                cityModel.CountryID = Convert.ToInt32(@dataRow["CountryID"]);
                cityModel.StateID = Convert.ToInt32(@dataRow["StateID"]);
                cityModel.CityCode = @dataRow["CityCode"].ToString();
            }

            #endregion

            return View(cityModel);
        }

        public IActionResult onSubmit(CityModel cityModel)
        {
            if (cityModel.CountryID <= 0)
            {
                ModelState.AddModelError("Country", "A valid Country is required.");
            }

            if (cityModel.StateID <= 0)
            {
                ModelState.AddModelError("State", "A valid State is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (cityModel.CityID == 0)
                {
                    command.CommandText = "PR_LOC_City_Insert";
                }
                else
                {
                    command.CommandText = "PR_LOC_City_Update";
                    command.Parameters.Add("@CityID", SqlDbType.Int).Value = cityModel.CityID;
                }
                command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = cityModel.CityName;
                command.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = cityModel.CityCode;
                command.Parameters.Add("@StateID", SqlDbType.Int).Value = cityModel.StateID;
                command.Parameters.Add("@CountryID", SqlDbType.Int).Value = cityModel.CountryID;
                command.ExecuteNonQuery();

                if (cityModel.CityID == 0)
                {
                    TempData["CityInsertMsg"] = "Record inserted successfully";
                }
                else
                {
                    TempData["CityUpdateMsg"] = "Record updated successfully";
                }

                return RedirectToAction("Index");
            }

            return View("CityAddEdit", cityModel);
        }

        #endregion
    }

}

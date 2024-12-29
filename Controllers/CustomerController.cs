using Microsoft.AspNetCore.Mvc;
using MVCDemo.Models;
using System.Data.SqlClient;
using System.Data;
using Irony.Parsing;

namespace MVCDemo.Controllers
{
    public class CustomerController : Controller
    {
        private IConfiguration configuration;

        public CustomerController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Index()
        {
            //int CustomerID = 601;
            //List<CustomerModel> c1 = new List<CustomerModel>();

            //for (int i = 1; i <= 5; i++)
            //{
            //    c1.Add(new CustomerModel(CustomerID, "abc", "address1", "abc", "1234567890", "1234", "city1", "360005", 5000, 201));
            //    CustomerID++;
            //}

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Cus_SelectAll";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }

        public IActionResult customerDelete(int CustomerID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "PR_Cus_Delete";
                sqlCommand.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CustomerID;
                sqlCommand.ExecuteNonQuery();
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Route("Customer/CustomerForm")]
        public IActionResult CustomerForm(int? CustomerID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);

            List<UserDropDownModel> userList = new List<UserDropDownModel>();

            foreach (DataRow data in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.Username = data["Username"].ToString();
                userList.Add(userDropDownModel);
            }
            ViewBag.UserList = userList;

            if (CustomerID == null || CustomerID==0)
            {
                return View(new CustomerModel());
            }

            #region CustomerByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Cus_SelectByID";
            command.Parameters.AddWithValue("@CustomerID", CustomerID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            CustomerModel customerModel = new CustomerModel();

            foreach (DataRow dataRow in table.Rows)
            {
                customerModel.CustomerID = Convert.ToInt32(@dataRow["CustomerID"]);
                customerModel.CustomerName = @dataRow["CustomerName"].ToString();
                customerModel.HomeAddress = @dataRow["HomeAddress"].ToString();
                customerModel.Email = @dataRow["Email"].ToString();
                customerModel.MobileNo = @dataRow["MobileNo"].ToString();
                customerModel.GSTNo = @dataRow["GSTNo"].ToString();
                customerModel.CityName = @dataRow["CityName"].ToString();
                customerModel.PinCode = @dataRow["PinCode"].ToString();
                customerModel.NetAmount = Convert.ToDouble(@dataRow["NetAmount"]);
                customerModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            return View(customerModel);
        }

        public IActionResult onSubmit(CustomerModel customerModel)
        {
            if (customerModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                if (customerModel.CustomerID <= 0)
                {
                    command.CommandText = "PR_Cus_Insert";
                }
                else
                {
                    command.CommandText = "PR_Cus_Update";
                    command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = customerModel.CustomerID;
                }
                command.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = customerModel.CustomerName;
                command.Parameters.Add("@HomeAddress", SqlDbType.VarChar).Value = customerModel.HomeAddress;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = customerModel.Email;
                command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = customerModel.MobileNo;
                command.Parameters.Add("@GSTNo", SqlDbType.VarChar).Value = customerModel.GSTNo;
                command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = customerModel.CityName;
                command.Parameters.Add("@PinCode", SqlDbType.VarChar).Value = customerModel.PinCode;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = customerModel.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = customerModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("Index");
            }

            return View("CustomerForm", customerModel);
        }
    }
}

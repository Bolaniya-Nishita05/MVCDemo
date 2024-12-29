using Microsoft.AspNetCore.Mvc;
using MVCDemo.Models;
using System.Data.SqlClient;
using System.Data;
using Irony.Parsing;

namespace MVCDemo.Controllers
{
    public class BillsController : Controller
    {
        private IConfiguration configuration;

        public BillsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Index()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Bills_SelectAll";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            return View(table);
        }

        public IActionResult billsDelete(int BillID)
        {
            try
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "PR_Bills_Delete";
                sqlCommand.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                sqlCommand.ExecuteNonQuery();
                return RedirectToAction("Index");
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [Route("Bills/BillsForm")]
        public IActionResult BillsForm(int? BillID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Order_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            List<OrderDropDownModel> orderList = new List<OrderDropDownModel>();
            foreach (DataRow data in dataTable1.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(data["OrderID"]);
                orderDropDownModel.OrderDate = Convert.ToDateTime(data["OrderDate"]);
                orderList.Add(orderDropDownModel);
            }
            ViewBag.OrderList = orderList;

            SqlCommand command2 = connection1.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_User_DropDown";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);

            List<UserDropDownModel> userList = new List<UserDropDownModel>();

            foreach (DataRow data in dataTable2.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                userDropDownModel.Username = data["Username"].ToString();
                userList.Add(userDropDownModel);
            }
            ViewBag.UserList = userList;

            if (BillID == null || BillID==0)
            {
                return View(new BillsModel { BillDate=DateTime.Now});
            }

            #region BillsByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectByID";
            command.Parameters.AddWithValue("@BillID", BillID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            BillsModel billModel = new BillsModel();

            foreach (DataRow dataRow in table.Rows)
            {
                billModel.BillID = Convert.ToInt32(@dataRow["BillID"]);
                billModel.BillNumber = @dataRow["BillNumber"].ToString();
                billModel.BillDate = Convert.ToDateTime(@dataRow["BillDate"]);
                billModel.OrderID = Convert.ToInt32(@dataRow["OrderID"]);
                billModel.TotalAmount = Convert.ToDouble(@dataRow["TotalAmount"]);
                billModel.Discount = Convert.ToDouble(@dataRow["Discount"]);
                billModel.NetAmount = Convert.ToDouble(@dataRow["NetAmount"]);
                billModel.UserID = Convert.ToInt32(@dataRow["UserID"]);
            }

            #endregion

            return View(billModel);
        }

        public IActionResult onSubmit(BillsModel billModel)
        {
            if (billModel.OrderID <= 0)
            {
                ModelState.AddModelError("OrderID", "A valid Order is required.");
            }

            if (billModel.UserID <= 0)
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
                if (billModel.BillID <= 0)
                {
                    command.CommandText = "PR_Bills_Insert";
                }
                else
                {
                    command.CommandText = "PR_Bills_Update";
                    command.Parameters.Add("@BillID", SqlDbType.Int).Value = billModel.BillID;
                }
                command.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = billModel.BillNumber;
                command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billModel.BillDate;
                command.Parameters.Add("@OrderID", SqlDbType.Int).Value = billModel.OrderID;
                command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billModel.TotalAmount;
                command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billModel.Discount;
                command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billModel.NetAmount;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = billModel.UserID;
                command.ExecuteNonQuery();
                return RedirectToAction("Index");
            }

            return View("BillsForm", billModel);
        }
    }
}

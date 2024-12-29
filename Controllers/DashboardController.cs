using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using MVCDemo.Models;

namespace MVCDemo.Controllers
{
    public class DashboardController : Controller
    {
        private IConfiguration configuration;

        public DashboardController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = new DashboardModel
            {
                Counts = new List<DashboardCountsModel>(),
                RecentOrders = new List<RecentOrderModel>(),
                RecentProducts = new List<RecentProductModel>(),
                TopCustomers = new List<TopCustomerModel>(),
                TopSellingProducts = new List<TopSellingProductModel>(),
                NavigationLinks = new List<QuickLinksModel>()
            };

            using (var connection = new SqlConnection(this.configuration.GetConnectionString("ConnectionString")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_GetDashboardData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            // Fetch counts
                            while (await reader.ReadAsync())
                            {
                                dashboardData.Counts.Add(new DashboardCountsModel
                                {
                                    Metric = reader["Metric"].ToString(),
                                    Value = Convert.ToInt32(reader["Value"])
                                });
                            }

                            // Fetch recent orders
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.RecentOrders.Add(new RecentOrderModel
                                    {
                                        OrderID = Convert.ToInt32(reader["OrderID"]),
                                        CustomerName = reader["CustomerName"].ToString(),
                                        OrderDate = Convert.ToDateTime(reader["OrderDate"])
                                    });
                                }
                            }

                            // Fetch recent products
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.RecentProducts.Add(new RecentProductModel
                                    {
                                        ProductID = Convert.ToInt32(reader["ProductID"]),
                                        ProductName = reader["ProductName"].ToString(),
                                        AddedDate = Convert.ToDateTime(reader["AddedDate"]),
                                        StockQuantity = Convert.ToInt32(reader["StockQuantity"])
                                    });
                                }
                            }

                            // Fetch top customers
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.TopCustomers.Add(new TopCustomerModel
                                    {
                                        CustomerName = reader["CustomerName"].ToString(),
                                        TotalOrders = Convert.ToInt32(reader["TotalOrders"]),
                                        Email = reader["Email"].ToString()
                                    });
                                }
                            }

                            // Fetch top selling products
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.TopSellingProducts.Add(new TopSellingProductModel
                                    {
                                        ProductName = reader["ProductName"].ToString(),
                                        TotalSoldQuantity = Convert.ToInt32(reader["TotalSoldQuantity"])
                                    });
                                }
                            }
                        }
                    }
                }
            }

            dashboardData.NavigationLinks = new List<QuickLinksModel> {
        new QuickLinksModel {ActionMethodName = "Index", ControllerName="Dashboard", LinkName="Dashboard" },
        new QuickLinksModel {ActionMethodName = "Index", ControllerName="Country", LinkName="Country" },
        new QuickLinksModel {ActionMethodName = "Index", ControllerName="State", LinkName="State" },
        new QuickLinksModel {ActionMethodName = "Index", ControllerName="City", LinkName="City" }
    };

            var model = new DashboardModel
            {
                Counts = dashboardData.Counts,
                RecentOrders = dashboardData.RecentOrders,
                RecentProducts = dashboardData.RecentProducts,
                TopCustomers = dashboardData.TopCustomers,
                TopSellingProducts = dashboardData.TopSellingProducts,
                NavigationLinks = dashboardData.NavigationLinks
            };

            return View(model);
        }
    }
}

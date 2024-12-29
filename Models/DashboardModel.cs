namespace MVCDemo.Models
{
    public class DashboardModel
    {
        public List<DashboardCountsModel> Counts { get; set; }
        public List<RecentOrderModel> RecentOrders { get; set; }
        public List<RecentProductModel> RecentProducts { get; set; }
        public List<TopCustomerModel> TopCustomers { get; set; }
        public List<TopSellingProductModel> TopSellingProducts { get; set; }
        public List<QuickLinksModel> NavigationLinks { get; set; }

    }
}

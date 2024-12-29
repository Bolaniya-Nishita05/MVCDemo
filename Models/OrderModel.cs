using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }

        [Required(ErrorMessage ="Order Date is required")]
        public DateTime OrderDate { get;set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public string PaymentMode {  get; set; }

        [Required]
        public double TotalAmount { get; set; }

        [Required]
        [MaxLength(100)]
        public string ShippingAddress {  get; set; }

        [Required]
        public int UserID { get; set; }
    }

    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

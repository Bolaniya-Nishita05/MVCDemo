using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class OrderDetailModel
    {
        public int OrderDetailID { get; set; }

        [Required(ErrorMessage ="OrderID is required")]
        public int OrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}

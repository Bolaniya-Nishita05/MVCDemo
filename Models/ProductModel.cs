using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage ="Product Name is required")]
        [MaxLength(50)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        [Display(Name ="Price")]
        public double ProductPrice { get; set; }

        [Required(ErrorMessage = "Product Code is required")]
        [MaxLength (10)]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Product Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "UserID is required")]
        public int UserID {  get; set; }

        //public ProductModel(int ProductID, string ProductName, double ProductPrice, string ProductCode, string Description, int UserID)
        //{
        //    this.ProductID = ProductID;
        //    this.ProductName = ProductName;
        //    this.ProductPrice = ProductPrice;
        //    this.ProductCode = ProductCode;
        //    this.Description = Description;
        //    this.UserID = UserID;
        //}
    }

    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class CustomerModel
    {
        public int CustomerID { get; set; }

        [Required]
        public string CustomerName {  get; set; }

        [Required]
        public string HomeAddress {  get; set; }

        [Required]
        [EmailAddress]
        public string Email {  get; set; }

        [Required]
        [Phone]
        public string MobileNo { get; set; }

        [Required]
        public string GSTNo {  get; set; }

        [Required]
        public string CityName {  get; set; }

        [Required]
        public string PinCode {  get; set; }

        [Required]
        public double NetAmount { get; set; }

        [Required]
        public int UserID { get; set; }
    }

    public class CustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}

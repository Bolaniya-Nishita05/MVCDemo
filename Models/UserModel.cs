using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class UserModel
    {
        public int UserID { get;set; }
        [Required(ErrorMessage ="Username is required")]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Phone]
        public string MobileNo { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int IsActive { get; set; }
    }

    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
    }

    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}

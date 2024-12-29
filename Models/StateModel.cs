using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MVCDemo.Models
{
    public class StateModel
    {
        public int StateID { get; set; }
        [Required]
        [DisplayName("State Name")]
        public string StateName { get; set; }
        [Required]
        [DisplayName("Country Name")]
        public int CountryID { get; set; }
        [Required]
        [DisplayName("State Code")]
        public string StateCode { get; set; }
    }

    public class StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
}

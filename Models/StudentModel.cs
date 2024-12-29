using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class StudentModel
    {
        public string StudentID { get; set; }

        [Required]
        public string StudentName { get; set; }
        [Required()]
        public string EnrollmentNo { get; set; }
        public int RollNo;
        public DateTime BirthDate;
        public int Age;
        [Required()]
        public string Password { get; set; }
        public int CurrentSem;

        [Required(ErrorMessage ="Cannot be empty")]
        [EmailAddress]
        public string EmailInstitute { get; set; }
        public string EmailPersonal { get; set; }

        [Required()]
        [MinLength(6)]
        public string ContactNo { get; set; }
        public int CastID;
        public int CityID;
        public string Remarks { get; set; }
    }
}

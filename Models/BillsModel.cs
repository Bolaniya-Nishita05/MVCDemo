﻿using System.ComponentModel.DataAnnotations;

namespace MVCDemo.Models
{
    public class BillsModel
    {
        public int BillID { get; set; }

        [Required(ErrorMessage ="BillNumber is required")]
        public string BillNumber { get; set; }

        [Required]
        public DateTime BillDate { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        [Required]
        public double Discount { get; set; }

        [Required]
        public double NetAmount { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}

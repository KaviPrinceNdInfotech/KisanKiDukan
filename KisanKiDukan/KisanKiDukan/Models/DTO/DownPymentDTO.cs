using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class DownPymentDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Merital Status")]
        public string MeritalStatus { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }
        [Required]
        [Display(Name = "Pin Code")]
        public Nullable<int> PinCode { get; set; }
        [Required]
        [Display(Name = "PAN/Aadhar Number")]
        public string PAN_Aadhar_Number { get; set; }
        [Required]
        [Display(Name = "Monthly Income")]
        public Nullable<decimal> MonthlyIncome { get; set; }
        [Required]
        [Display(Name = "product Id")]
        public Nullable<int> product_Id { get; set; }
    }
}
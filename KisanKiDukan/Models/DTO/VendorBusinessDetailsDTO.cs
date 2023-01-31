using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.DTO
{
    public class VendorBusinessDetailsDTO
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets of Company Name")]
        public string CompanyName { get; set; }
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets of Company Name")]
        public string LegalCompanyName { get; set; }
        public string BusinessFilingStatus { get; set; }
        [Required]
        [RegularExpression("([A-Z0-9 .&'-]+)", ErrorMessage = "Enter only alphabets of Company Name")]
        public string PAN_No { get; set; }
        [Required]
        public string RegisteredAddress { get; set; }
        public string OperatingAddress { get; set; }
        public SelectList BusinessStatuList { get; set; }
    }
}
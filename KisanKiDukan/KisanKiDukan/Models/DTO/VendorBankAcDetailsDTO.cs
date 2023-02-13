using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class VendorBankAcDetailsDTO
    {
        [Required]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets of Pay To Name")]
        public string PayToName { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        [RegularExpression(@"^\d{9,18}$", ErrorMessage = "Invalid account number")]
        public string AccountNumber { get; set; }
        [Required]
        public string IFSC_Code { get; set; }
        [Required]
        public string BranchAddress { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class CustomerDTO
    {
        public int User_Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email_Id { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Address { get; set; }
        public int Refer_Id { get; set; }
        public string Rgx { get; set; }
        public double Wallet_Amount { get; set; }
        [Required]
        public string Pincode { get; set; }
    }
}
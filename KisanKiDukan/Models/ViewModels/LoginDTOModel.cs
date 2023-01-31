using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class LoginDTOModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email_Id { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string url { get; set; }
    }
    public class LoginOTP
    {
        public string Mobile { get; set; }
    }
}
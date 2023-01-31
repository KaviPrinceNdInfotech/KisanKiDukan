using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ForgotPWD
    {
        [Required]
        public string Email_Id { get; set; }
    }
    public class verifyEmailDTO
    {
        public int OTP { get; set; }
    }
}
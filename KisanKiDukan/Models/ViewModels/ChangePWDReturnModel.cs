using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ChangePWDReturnModel
    {
        public string Email_Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
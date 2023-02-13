using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ForgotReturnModel
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public string Email_Id { get; set; }
        public string NewPassword { get; set; }
    }
}
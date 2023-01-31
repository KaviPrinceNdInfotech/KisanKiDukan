using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ForgotReturnUserModel
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public string Email_Id { get; set; }
        public Customer User_Detail { get; set; }
    }
}
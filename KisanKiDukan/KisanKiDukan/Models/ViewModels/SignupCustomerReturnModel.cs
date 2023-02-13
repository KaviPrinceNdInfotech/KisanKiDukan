using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class SignupCustomerReturnModel
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public int User_Id { get; set; }
        public string Phone { get; set; }
        public string Email_Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}
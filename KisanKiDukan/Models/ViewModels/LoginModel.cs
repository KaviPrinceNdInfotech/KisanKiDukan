using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public Nullable<int> VendorId { get; set; }
    }
}
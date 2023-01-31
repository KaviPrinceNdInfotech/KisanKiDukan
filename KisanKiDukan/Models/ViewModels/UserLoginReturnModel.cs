using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class UserLoginReturnModel
    {
        public Customer User_Detail { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
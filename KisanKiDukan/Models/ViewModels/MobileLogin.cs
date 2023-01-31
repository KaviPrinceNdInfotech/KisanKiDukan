using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class MobileLogin
    {
        public string MobileOrEmail { get; set; }
        public int Otp { get; set; }
    }
}
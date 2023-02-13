using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class MembershipUserShowModel_Admin
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string FullName { get; set; }
        public string Email_Id { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public double Mebr_Amount { get; set; }
    }
}
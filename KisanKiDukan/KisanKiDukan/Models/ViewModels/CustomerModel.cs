using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class CustomerModel
    {
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Email_Id { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public double WalletAmount { get; set; }
        public Nullable<bool> IsPremiumMember { get; set; }
        public Nullable<System.DateTime> PremiumMemberOn { get; set; }
        public string StrwalletAmount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class MemberDetailShowAdmin
    {
        public int Id { get; set; }
        public int Refer_Id { get; set; }
        public int Login_Id { get; set; }
        public string FullName { get; set; }
        public string Email_Id { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? CreateDate { get; set; }
        public double Wallet_Amount { get; set; }
    }
}
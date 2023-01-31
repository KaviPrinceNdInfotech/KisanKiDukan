using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class JoinMemberModelAdmin
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string FullName { get; set; }
        public string Email_Id { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Total_Member { get; set; }
        public double Wallet_Amount { get; set; }
    }
}
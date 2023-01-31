using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class MembershipDTO
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public double Percentage { get; set; }
        public double BenifitAmount { get; set; }
        public string Mobile_No { get; set; }
        public int DistributionMonth { get; set; }
        public string Remark { get; set; }
        public int Customer_Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string FullName { get; set; }
        public string Email_Id { get; set; }
        public string Phone { get; set; }
    }
}
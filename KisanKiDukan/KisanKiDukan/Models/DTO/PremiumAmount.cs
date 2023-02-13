using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class PremiumAmount
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int TillDate { get; set; }
    }
}
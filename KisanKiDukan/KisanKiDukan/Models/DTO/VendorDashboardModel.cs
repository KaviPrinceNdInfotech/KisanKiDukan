using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class VendorDashboardModel
    {
        public double? TotalRevenue { get; set; }
        public int? TotalSale { get; set; }
        public List<OrderData> OrderDataList { get; set; }
    }
}
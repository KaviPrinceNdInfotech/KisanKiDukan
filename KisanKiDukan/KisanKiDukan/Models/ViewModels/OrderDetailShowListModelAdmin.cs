using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.ViewModels
{
    public class OrderDetailShowListModelAdmin
    {
        public IEnumerable<OrderDetailShowModelAdmin> OrderDetail { get; set; }
        public double Total_Price { get; set; }
        public SelectList OrderStatusList { get; set; }
    }
}
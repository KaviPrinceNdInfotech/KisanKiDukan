using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.ViewModels
{
    public class OrderShowListModelAdmin
    {
        public IEnumerable<OrderShowModelAdmin> Order { get; set; }
        public int? Page { get; set; }
        public int? NumberOfPages { get; set; }
        public SelectList OrderStatusList { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.DTO
{
    public class PAvailDTO
    {
        public int Id { get; set; }
        public int? Product_Id { get; set; }
        public int? Metrics_Id { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
        public int? Quantity { get; set; }
        public Nullable<int> DiscountPrice { get; set; }
        //public int? Weight { get; set; }
        public string Metrics { get; set; }
        public double? OurPrice { get; set; }
        public SelectList MetricList { get; set; }
        public SelectList StoreList { get; set; }
    }
}
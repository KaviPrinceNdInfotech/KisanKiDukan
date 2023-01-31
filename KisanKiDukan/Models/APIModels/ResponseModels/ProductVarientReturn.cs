using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.APIModels.ResponseModels
{
    public class ProductVarientReturn
    {
        public int Id { get; set; }
        public int? Product_Id { get; set; }
        public int? Metrics_Id { get; set; }
        public double Price { get; set; }
        public int? Weight { get; set; }
        public string Metrics { get; set; }
        public double? OurPrice { get; set; }
        public int Qty { get; set; }
        public bool StockAvailability { get; set; }
    }
}
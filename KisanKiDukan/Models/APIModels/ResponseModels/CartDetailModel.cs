using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.APIModels.ResponseModels
{
    public class CartDetailModel
    {
        public int CartDetail_Id { get; set; }
        public int Product_Id { get; set; }
        public string ProductName { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }
        public int Metric_Id { get; set; }
        public string Metric { get; set; }
        public double Price { get; set; }
        public string ProductImage { get; set; }
        public int? Category_Id { get; set; }
        public Nullable<int> VendorId { get; set; }
       
    }
}
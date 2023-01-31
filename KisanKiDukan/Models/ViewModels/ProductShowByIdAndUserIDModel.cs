using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ProductShowByIdAndUserIDModel
    {
        public int Product_Id { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string UpToText { get; set; }
        public Nullable<double> Price { get; set; }
        public string ProductDescription { get; set; }
        public string Metrics { get; set; }
        public bool StockAvailability { get; set; }
        public int Quantity { get; set; }
        public int? Weight { get; set; }
        public bool IsFreeShipping { get; set; }
        public double ShippingCharge { get; set; }
        public int Metric_Id { get; set; }
        public Nullable<double> OurPrice { get; set; }
        public bool IsVariant { get; set; }
        public int Qty { get; set; }
        public Nullable<int> SubId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string VendorName { get; set; }
        public List<VariantModels> Variant { get; set; }
    }
    public class VariantModels
    {
        public int Qty { get; set; }
        public int Variant_Id { get; set; }
        public string Metric { get; set; }
        public int? Weight { get; set; }
        public int? Metric_Id { get; set; }
        public int Product_Id { get; set; }
        public double? Price { get; set; }
        public bool IsStock { get; set; }
        public Nullable<double> OurPrice { get; set; }
        public Nullable<int> VendorId { get; set; }
    }
}
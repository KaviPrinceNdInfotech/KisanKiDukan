using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ProductShowByCategoriesModel
    {
        public int Product_Id { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> Category_Id { get; set; }
        public string ProductImage { get; set; }
        public string CategoryImage { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryImage { get; set; }
        public string SubCategoryName { get; set; }
        public string UpToText { get; set; }
        public Nullable<double> Price { get; set; }
        public string ProductDescription { get; set; }
        public string Metrics { get; set; }
        public bool StockAvailability { get; set; }
        public int? Weight { get; set; }
        public bool IsFreeShipping { get; set; }
        public double ShippingCharge { get; set; }
        public int? Metrics_Id { get; set; }
        public Nullable<double> OurPrice { get; set; }
        public bool IsVariant { get; set; }
        public int Qty { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string VendorName { get; set; }
    }
}
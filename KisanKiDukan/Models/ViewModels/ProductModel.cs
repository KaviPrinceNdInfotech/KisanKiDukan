using KisanKiDukan.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.ViewModels
{
    public class ProductModel
    {
        public int? subId { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter product name")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets of Product Name")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Select Category first")]
        public Nullable<int> Category_Id { get; set; }
        public string ProductImage { get; set; }
        public string ImageBase64 { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public SelectList CategoryList { get; set; }
        //[Required(ErrorMessage = "Select Metrics")]
        public int? Metric_Id { get; set; }
        public string Metrics { get; set; }
        public SelectList MetricList { get; set; }
        public SelectList StoreList { get; set; }
        public string CategoryName { get; set; }
        public SelectList SubCategory { get; set; }
        [Required(ErrorMessage = "Enter Price")]
        [RegularExpression("^[0-9]$", ErrorMessage = "Price must be numeric")]
        public Nullable<double> Price { get; set; }
        public string ProductDescription { get; set; }
        public bool IsStock { get; set; }
        public string IsStocks { get; set; }
        public Nullable<int> OfferValue { get; set; }
        public Nullable<int> OfferType { get; set; }
        public IEnumerable<SelectListItem> OfferTypeList { get; set; }
        public List<PAvailDTO> PAvail { get; set; }
        public string Page_Url { get; set; }
        public bool IsReplacement { get; set; }
        public string ReplacementTC { get; set; }
        [Required(ErrorMessage = "Required Quantity")]
        public Nullable<int> Quantity { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsFreeShipping { get; set; }
        public double ShippingCharge { get; set; }
        public bool IsReviewsAllow { get; set; }
        public string SKU { get; set; }
        public Nullable<int> Brand_Id { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> DiscountCode_Id { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        //public Nullable<int> Ourprice { get; set; }
        public Nullable<double> OurPrice { get; set; }
        public string Barcode { get; set; }
        public string HsnCode { get; set; }
        public bool IsVariant { get; set; }
        public bool IsRecomend { get; set; }
        public bool IsHotdeals { get; set; }
        public bool IsFeatureProduct { get; set; }
        public bool IsSpecial { get; set; }
        public List<VariantModel> Variants { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<double> PremiumAmount { get; set; }
        public Nullable<double> CGST { get; set; }
        public Nullable<double> SGST { get; set; }
        public Nullable<double> IGST { get; set; }

        public string VendorName { get; set; }
        public List<string> multipleImage { get; set; }

        public List<ProductImageModel> ProductImageList { get; set; }
        public List<HttpPostedFileBase> MultiImageFile { get; set; }
    }
    public class VariantModel
    {
        public int Variant_Id { get; set; }
        public string Metric { get; set; }
        public int Weight { get; set; }
        public int Metric_Id { get; set; }
        public int Product_Id { get; set; }
        public double Price { get; set; }
        public bool IsStock { get; set; }
        
    }
}
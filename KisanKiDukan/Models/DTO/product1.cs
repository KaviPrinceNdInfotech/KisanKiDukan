using KisanKiDukan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class Product1
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter product name")]
        [RegularExpression("([a-zA-Z .&'-]+)", ErrorMessage = "Enter only alphabets of Product Name")]
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ImageBase64 { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Enter Price")]
        [RegularExpression("^[0-9]$", ErrorMessage = "Price must be numeric")]
        public Nullable<double> Price { get; set; }
        public string ProductDescription { get; set; }
        public bool IsStock { get; set; }
        public string IsStocks { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> VendorId { get; set; }
        public bool IsReviewsAllow { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        //public Nullable<int> Ourprice { get; set; }
        public Nullable<double> OurPrice { get; set; }
        public string CategoryImage { get; set; }
        public string VendorName { get; set; }


        public List<string> multipleImage { get; set; }

        //public List<ProductImageModel> ProductImageList { get; set; }
        public List<HttpPostedFileBase> MultiImageFile { get; set; }

    }
}
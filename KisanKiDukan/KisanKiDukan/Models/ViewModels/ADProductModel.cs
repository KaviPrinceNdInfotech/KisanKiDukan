using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ADProductModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter AD Title")]
        public string ADTitle { get; set; }
        public string Description { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} must be a Number.")]
        public Nullable<double> Price { get; set; }
        [Required(ErrorMessage = "Please upload atleast one image")]
        public string ADImage { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public List<ADProductDetailModel> ADProductDetailList { get; set; }
        public List<HttpPostedFileBase> MultiImageFile { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class BrandDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter brand name")]
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public bool IsActive { get; set; }
        public string Page_Url { get; set; }
        public bool IsFeature { get; set; }
        public HttpPostedFileBase Logo { get; set; }
    }
}
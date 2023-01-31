using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class BannerAddDTO
    {
        public int Id { get; set; }
        public string BannerPath { get; set; }
        //[Required(ErrorMessage = "Cant be empty this field")]
        public Nullable<int> OfferValue { get; set; }
        //[Required(ErrorMessage = "Select Category first")]
        //public Nullable<int> Category_Id { get; set; }
        [Required(ErrorMessage = "Cant be empty this")]
        public HttpPostedFileBase ImageFile { get; set; }
        //public SelectList CategoryList { get; set; }
        //public string CategoryName { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
    }
}
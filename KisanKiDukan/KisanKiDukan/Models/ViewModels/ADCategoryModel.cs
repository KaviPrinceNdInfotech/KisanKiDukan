using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.ViewModels
{
    public class ADCategoryModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter category name")]
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentCategory { get; set; }
        public string CategoryImage { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public bool IsActive { get; set; }
        public string Page_Url { get; set; }
        public bool IsFeature { get; set; }
        public SelectList CategoryList { get; set; }
        public string UpToText { get; set; }
        public Nullable<int> MainCat_Id { get; set; }
    }
}
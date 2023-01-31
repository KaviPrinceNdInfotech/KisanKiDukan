using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class SubCategorymodal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CatImage { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string UpToText { get; set; }
        public Nullable<int> MainCat_Id { get; set; }
        public int totalProd { get; set; }
    }
}
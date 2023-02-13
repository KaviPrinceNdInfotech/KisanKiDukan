using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ADSubCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CatImage { get; set; }
        public string UpToText { get; set; }
        public Nullable<int> MainCat_Id { get; set; }
    }
}
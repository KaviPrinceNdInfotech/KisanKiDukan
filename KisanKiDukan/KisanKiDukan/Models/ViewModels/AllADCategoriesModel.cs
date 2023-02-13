using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class AllADCategoriesModel
    {
        public List<ADCategoryModel> ADCategoryList { get; set; }
        public List<ADSubCategoryModel> SubCategoryList { get; set; }
    }
}
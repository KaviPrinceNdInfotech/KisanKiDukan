using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class AllCategoryModel
    {
        public List<CategoryModel> CategoryLists { get; set; }
        public List<SubCategorymodal> SubCategoryLists { get; set; }
    }
}
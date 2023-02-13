using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.APIModels
{
    public class CategoryApiModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
        public string UpToText { get; set; }
        public IEnumerable<SubCategories> SubCategories { get; set; }
    }
    public class SubCategories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MainCat_Id { get; set; }
        public string SubCategroyImage { get; set; }
        public string UpToText { get; set; }
    }
}
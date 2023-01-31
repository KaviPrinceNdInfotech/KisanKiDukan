using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ProductShowListModel
    {
        public IEnumerable<ProductShowByIdAndUserIDModel> GetProducts { get; set; }
        public IEnumerable<ProductShowByCategoriesModel> Products { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
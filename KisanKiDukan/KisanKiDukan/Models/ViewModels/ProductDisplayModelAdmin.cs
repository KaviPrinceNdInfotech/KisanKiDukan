using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ProductDisplayModelAdmin
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public int? Page { get; set; }
        public int? NumberOfPages { get; set; }
        public int Category_Id { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.APIModels.ResponseModels
{
    public class CategoriesApiReturnModel
    {
        public IEnumerable<CategoryApiModel> Category { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
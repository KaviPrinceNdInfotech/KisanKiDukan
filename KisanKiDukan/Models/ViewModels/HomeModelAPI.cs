using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class HomeModelAPI
    {
        public IEnumerable<ProductModel> HotDeals { get; set; }

        public IEnumerable<ProductModel> SpecialDeals { get; set; }
        public IEnumerable<ProductModel> NewArrivalDeals { get; set; }
        public IEnumerable<ProductModel> ReccomendedDeals { get; set; }
        public IEnumerable<ProductModel> OrganicDeals { get; set; }

    }
}
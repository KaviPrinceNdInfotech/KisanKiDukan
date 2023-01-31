using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class HomeModel
    {
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<ProductModel> newProducts { get; set; }
        public IEnumerable<ProductModel> RecommendProducts { get; set; }
        public IEnumerable<ProductModel> HotProducts { get; set; }
        public IEnumerable<ProductModel> FeatureProducts { get; set; }
        public IEnumerable<ProductModel> FeatureProducts1 { get; set; }
        public IEnumerable<ProductModel> specialProducts { get; set; }
        public IEnumerable<ProductModel> NewProducts { get; set; }
        public IEnumerable<BannerImage> Banners { get; set; }
        public IEnumerable<promotionalbanner> ProBanners { get; set; }
    }
}
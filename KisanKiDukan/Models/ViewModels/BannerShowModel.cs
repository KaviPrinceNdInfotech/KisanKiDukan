using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class BannerShowModel
    {
        public IEnumerable<BannersModel> Banner { get; set; }
        public int? Page { get; set; }
        public int? NumberOfPages { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
    public class BannersModel
    {
        public int Id { get; set; }
        public string BannerPath { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ADProductDetailModel
    {
        public int Id { get; set; }
        public Nullable<int> ADProductId { get; set; }
        public string ADImages { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ProductImageModel
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        public string ImageName { get; set; }
        public Nullable<bool> IsLead { get; set; }
        public Nullable<int> AttrValue_Id { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.ViewModels
{
    public class ContentPageModel
    {
        public int Id { get; set; }
        public Nullable<int> Menu_Id { get; set; }
        [Required, AllowHtml]
        public string Content { get; set; }
        public string PageImage { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ReviewModel
    {
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewDesc { get; set; }
        public string ImagePathNew { get; set; }
        public HttpPostedFileBase ImagePath { get; set; }
        public Nullable<System.DateTime> SubmitDate { get; set; }
    }
}
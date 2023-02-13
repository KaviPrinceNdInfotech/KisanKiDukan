using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class Rating1
    {

        public Nullable<int> ProductId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewDesc { get; set; }
        public string Image { get; set; }
        public Nullable<System.DateTime> SubmitDate { get; set; }

        public string ImageName { get; set; }
        public string ImageBase { get; set; }

    }
}
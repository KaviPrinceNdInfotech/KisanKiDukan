using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class promotionalbannerDTO
    {
        public int Id { get; set; }
        public string promotionalbannerpath { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
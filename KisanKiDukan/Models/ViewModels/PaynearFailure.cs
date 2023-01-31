using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class PaynearFailure
    {
        public string amount { get; set; }
        public string referenceNo { get; set; }
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
    }
}
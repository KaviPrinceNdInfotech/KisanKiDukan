using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class PaynearResponseModel
    {
        public string orderRefNo { get; set; }
        public string amount { get; set; }
        public string paymentId { get; set; }
        public string merchantId { get; set; }
        public string transactionId { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public string transactionType { get; set; }
        public string transactionDate { get; set; }
        public string currencyCode { get; set; }
        public string paymentMethod { get; set; }
        public string paymentBrand { get; set; }
    }
}
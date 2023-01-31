using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class PayneerForm
    {
        public string referenceNo { get; set; }
        public int outletId { get; set; }

        public string apiVersion { get; set; }
        public string currencyCode { get; set; }
        public string locale { get; set; }
        public string description { get; set; }
        public double amount { get; set; }
        public string responseURL { get; set; }
        public int channel { get; set; }
        public string billingContactName { get; set; }
        public string billingAddress { get; set; }
        public string billingCity { get; set; }
        public string billingState { get; set; }
        public string billingPostalCode { get; set; }
        public string billingCountry { get; set; }
        public string billingEmail { get; set; }
        public string billingPhone { get; set; }
        public string shippingContactName { get; set; }
        public string shippingAddress { get; set; }
        public string shippingCity { get; set; }
        public string shippingState { get; set; }
        public string shippingPostalCode { get; set; }
        public string shippingCountry { get; set; }
        public string shippingEmail { get; set; }
        public string shippingPhone { get; set; }
    }
}
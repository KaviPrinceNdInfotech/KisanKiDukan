using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string ProductName { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public string TaxRate { get; set; }
        public string TaxType { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string message { get; set; }
        public Nullable<int> VendorId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class GenerateInvoiceViewModel
    {
        public IEnumerable<OrderDetailModel> OrderDetailData { get; set; }
        public int OrderId { get; set; }
        public double FinalPrice { get; set; }
        public string VendorName { get; set; }
        public string ContactNumber { get; set; }
        public string EmailId { get; set; }
        public string CompanyName { get; set; }
        public string Adress { get; set; }
        public string city { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string Invoice_No { get; set; }
    }
    public class InvoiceDetailModel
    {
        public string ProductName { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public string TaxRate { get; set; }
        public string TaxType { get; set; }
    }
}
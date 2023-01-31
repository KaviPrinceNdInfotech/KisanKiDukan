using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.DTO
{
    public class ProductSaleModel
    {
        public int Id { get; set; }
        public int PaymentType_Id { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> Vendor_Id { get; set; }
        public int Wheight { get; set; }
        public double TotalPrice { get; set; }
        public SelectList MetricList { get; set; }
        public SelectList PaymentTypes { get; set; }
        public SelectList VendorList { get; set; }
        public int Product_Id { get; set; }
        public int Client_Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string PONumber { get; set; }
        public string ModeOfDelivery { get; set; }
        public string WayBillNo { get; set; }
        public string Remark { get; set; }
        public double? AdittionalCharge { get; set; }
        public int StateId { get; set; }
        public SelectList States { get; set; }
        public List<ProductData> Products { get; set; }
        public ProductSaleModel()
        {
            Products = new List<ProductData> { new ProductData { Product_Id = 0, Wheight = 0, Stock = 0, Metric_Id = 0, Quantity = 0, Price = 0 } };
        }
    }
    public class ProductData
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        public int? PurchaseOrder_Id { get; set; }
        public string ProductName { get; set; }
        public string MetricName { get; set; }
        public int Stock { get; set; }
        public Nullable<int> Store_Id { get; set; }
        public int Wheight { get; set; }
        public Nullable<int> Metric_Id { get; set; }
        public Nullable<int> Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public double FinalPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public SelectList MetricList { get; set; }
        public Nullable<int> Vendor_Id { get; set; }
        public Nullable<int> POD_Id { get; set; }
        public bool IsChecked { get; set; }
        public double TotalQuantity { get; set; }
        public int LeftQuantity { get; set; }
        public int PoInvoice_Id { get; set; }
    }
}
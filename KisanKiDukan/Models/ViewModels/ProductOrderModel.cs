using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class ProductOrderModel
    {
        public int Order_Id { get; set; }
        public int Product_Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Metrics { get; set; }
        //public double Total_Price { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PaymentMode { get; set; }
        public double FinalPrice { get; set; }
        public double Product_Price { get; set; }
        public string OrderStatus { get; set; }
    }
}
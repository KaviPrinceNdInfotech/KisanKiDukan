using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class CreateOrderAPI
    {
        public int Id { get; set; }
        public int PaymentTypeId { get; set; }
        public int Product_Id { get; set; }
        public int User_Id { get; set; }
        public double FinalPrice { get; set; }
        public double Product_Price { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> Order_Id { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public string Metrics { get; set; }
        public double Weight { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<int> OrderStatus_Id { get; set; }
    }
}
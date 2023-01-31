using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class OrderDetailShowModelAdmin
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        public int User_Id { get; set; }
        public double Product_Price { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public bool IsDelivered { get; set; }
        public bool IfReturned { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Order_Id { get; set; }
        public double FinalPrice { get; set; }
        public int Quantity { get; set; }
        public string Metrics { get; set; }
        public int Category_Id { get; set; }
        public bool IsCancel { get; set; }
        public int OrderStatus_Id { get; set; }
        public string OrderStatus { get; set; }
        public double Total_Price { get; set; }
        public DateTime? OrderStatusDate { get; set; }
        public double Weight { get; set; }
    }
}
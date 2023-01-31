using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        public int Client_Id { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public bool IsDelivered { get; set; }
        public bool IfReturned { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> Order_Id { get; set; }
        public string ImageName { get; set; }
        public int? Quantity { get; set; }
        public Nullable<int> OrderStatus_Id { get; set; }
        public Nullable<bool> IsCancel { get; set; }
        public string OrderStatus { get; set; }
        public Nullable<int> VendorId { get; set; }
    }
}
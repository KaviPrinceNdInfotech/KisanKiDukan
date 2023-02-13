using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class CODOrderDTO
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        public bool IsCancel { get; set; }
        public double Price { get; set; }
        public double Total_Price { get; set; }
        public string PaymentMode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        //public bool IsDelivered { get; set; }
        //public bool IfReturned { get; set; }
        public string ProductName { get; set; }
        public int Order_Id { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public double FinalPrice { get; set; }
        public int Total_Items { get; set; }
    }
}
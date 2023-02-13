using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class OrderData
    {
        public int Id { get; set; }
        public System.DateTime OrderDate { get; set; }
        public double Total { get; set; }
        public int OrderStatus_Id { get; set; }
        public int PaymentType_Id { get; set; }
        public bool PaymentStatus { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Client_Id { get; set; }
        public string PinCode { get; set; }
        public string PaymentMode { get; set; }
        public long Transaction_No { get; set; }
        public bool IsCancel { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsReturned { get; set; }
        public Nullable<int> DeliveryOption_Id { get; set; }
        public string PrivateNote { get; set; }
        public double DeliveryCharges { get; set; }
        public Nullable<System.DateTime> StatusUpdateDate { get; set; }
        public string OrderStatus { get; set; }
    }
}
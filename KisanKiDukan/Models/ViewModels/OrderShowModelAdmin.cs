using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class OrderShowModelAdmin
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double Total_Price { get; set; }
        public int OrderStatus_Id { get; set; }
        public string OrderStatus { get; set; }
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
        public bool IsCancel { get; set; }
        public int Total_Items { get; set; }
        public long Transaction_No { get; set; }
        public DateTime? StatusUpdateDate { get; set; }
        public DateTime? ScheduledDeliveryDate { get; set; }
        public int? SlotCode { get; set; }
        public string SlotTiming { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string VendorName { get; set; }
    }
}
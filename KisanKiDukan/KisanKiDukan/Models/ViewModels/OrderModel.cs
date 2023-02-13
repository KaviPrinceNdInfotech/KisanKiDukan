using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class OrderModel
    {
        public List<CreateOrderAPI> OrderData { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string Shipping_Address { get; set; }
        public string Alternate_No { get; set; }
        public double Total_Price { get; set; }
        public double Mebr_Amount { get; set; }
        public int User_Id { get; set; }
        public double DeliveryCharges { get; set; }
        public DateTime? SchedultedDate { get; set; }
        public string SchedultedTimeSlot { get; set; }
        public int? SlotCode { get; set; }
        public Nullable<int> VendorId { get; set; }
        //----
        public DateTime OrderDate { get; set; }
    }
}
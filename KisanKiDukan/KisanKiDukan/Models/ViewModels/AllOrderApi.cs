using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class AllOrderApi
    {
        public int Order_Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double Grand_Total { get; set; }
        public string AlternateNumber { get; set; }
        public string Shipping_Address { get; set; }
        //public int User_Id  { get; set; }
        public int Total_Items { get; set; }
        public string PaymentMode { get; set; }
        public double DeliveryCharges { get; set; }
        public bool IsCancel { get; set; }
        public DateTime? CancellationDate { get; set; }
        public DateTime? ScheduledDeliveryDate { get; set; }
        public string TimeSlot { get; set; }
        public string OrderStatus { get; set; }
    }
}
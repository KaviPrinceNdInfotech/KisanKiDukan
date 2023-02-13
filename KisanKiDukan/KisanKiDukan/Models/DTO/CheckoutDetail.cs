using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class CheckoutDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public double? Total { get; set; }
        public string PurchasedItems { get; set; }
        public string SuccessUrl { get; set; }
        public string FailureUrl { get; set; }
        public int OrderId { get; set; }
        //--------------------------------------------
        public string razorId { get; set; }
        public string razorpayKey { get; set; }
        public string currency { get; set; }
    }
}
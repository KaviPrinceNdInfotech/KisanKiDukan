using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class UserOrderDetailModel
    {
        public IEnumerable<ProductOrderModel> OrderDetail { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public double Total_Price { get; set; }
        public double DeliveryCharges { get; set; }
    }
}
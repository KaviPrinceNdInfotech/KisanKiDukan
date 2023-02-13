using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class OrderSuccessViewModel
    {
        public int Id { get; set; }
        public int Order_No { get; set; }
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }
        public string PaymentMode { get; set; }
        public List<InvoiceModel> InvoiceData { get; set; }
        public OrderModel OrderData { get; set; }
        public List<OrderDetailModel> OrderDetail { get; set; }
    }
}
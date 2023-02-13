using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class OrderDetailRm
    {
        public List<OrderDetailModel> OrderDetail { get; set; }
    }
    public class OrderDetailModel
    {
        public int OrderDetailId { get; set; }
        public double Price { get; set; }
        public double FinalPrice { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Metrics { get; set; }
        public double Weight { get; set; }
        public bool IsCancel { get; set; }
        public string CancellationDate { get; set; }
        public string StatusName { get; set; }
    }
}
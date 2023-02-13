using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class KartDetailsDTO
    {
        public int Id { get; set; }
        public int Kart_Id { get; set; }
        public int PID { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<int> Metric_Id { get; set; }
        public bool IsVariant { get; set; }
        public double Volume { get; set; }
    }
}
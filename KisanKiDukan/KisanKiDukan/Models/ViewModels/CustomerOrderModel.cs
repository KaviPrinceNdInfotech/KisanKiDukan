using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class CustomerOrderModel
    {
        public IEnumerable<CODOrderDTO> Data { get; set; }
        public int? NumberOfPages { get; set; }
        public int? Page { get; set; }
        public double Total_Price { get; set; }
    }
}
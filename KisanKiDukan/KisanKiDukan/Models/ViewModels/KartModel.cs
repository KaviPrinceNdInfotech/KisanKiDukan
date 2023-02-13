using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class KartModel
    {
        public IEnumerable<KartDetailsDTO> Data { get; set; }
        public double? Total { get; set; }
    }
}
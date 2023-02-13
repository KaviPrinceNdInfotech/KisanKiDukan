using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class MetricsShowAdminModel
    {
        public IEnumerable<Metric> Metric { get; set; }
        public int? Page { get; set; }
        public int? NumberOfPages { get; set; }
    }
}
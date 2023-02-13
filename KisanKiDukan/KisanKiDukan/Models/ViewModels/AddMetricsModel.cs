using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class AddMetricsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field can't blank")]
        public string Metrics { get; set; }
        public int MetricCode { get; set; }
    }
}
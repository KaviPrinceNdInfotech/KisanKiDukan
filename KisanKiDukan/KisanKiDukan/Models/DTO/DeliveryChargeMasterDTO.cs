using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class DeliveryChargeMasterDTO
    {
        public int Id { get; set; }
        [Required]
        public double DeliveryCharge { get; set; }
        [Required]
        public double MinAmt { get; set; }
    }
}
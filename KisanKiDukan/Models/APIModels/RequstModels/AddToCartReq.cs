using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.APIModels.RequstModels
{
    public class AddToCartReq
    {
        [Required]
        public int User_Id { get; set; }
        [Required]
        public int Product_Id { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public int? Metric_Id { get; set; }
        public Nullable<int> VendorId { get; set; }
    }
    public class UpdateCarReq
    {
        [Required]
        public int User_Id { get; set; }
        [Required]
        public int Product_Id { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public int? Metric_Id { get; set; }
        [Required]
        public string Type { get; set; }
        public Nullable<int> VendorId { get; set; }
    }
}
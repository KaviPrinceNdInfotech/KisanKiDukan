using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class DeliveryOptionDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter delivery option")]
        public string OptionName { get; set; }
    }
}
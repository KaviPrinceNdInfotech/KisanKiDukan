using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class DeliveryPincodeDTO
    {
        public int Id { get; set; }
        [Required]
        public string Pincode { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public List<DeliveryPincode> PincodeList { get; set; }
    }
}
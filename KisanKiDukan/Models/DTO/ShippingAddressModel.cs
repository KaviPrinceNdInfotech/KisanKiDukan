using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class ShippingAddressModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Address { get; set; }

        public string BlockNo { get; set; }

        public string Email { get; set; }
        public string PinCode { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Location { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
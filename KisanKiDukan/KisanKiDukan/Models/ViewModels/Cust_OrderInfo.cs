
using KisanKiDukan.Models.APIModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.ViewModels
{
    public class Cust_OrderInfo
    {
        [Required(ErrorMessage = "Please enter Name.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter email id.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter mobile number.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter address.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter city name.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter state name.")]
        public string State { get; set; }
        public Nullable<int> Client_Id { get; set; }
        [Required(ErrorMessage = "Please enter pincode.")]
        public string PinCode { get; set; }
        public SelectList paymentMode { get; set; }
        [Required(ErrorMessage = "Please enter select payment type.")]
        public int paymentType { get; set; }
        public int paymentGateway { get; set; }
      
    }
}
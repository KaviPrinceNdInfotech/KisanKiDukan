using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class DCMasterDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field cant be empty")]
        public string DCName { get; set; }
        [Required(ErrorMessage = "Please Select type")]
        public string ShortCode { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class AddLocationDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Cant be null this field")]
        public string LocationName { get; set; }
        public string TimeDay { get; set; }
    }
}
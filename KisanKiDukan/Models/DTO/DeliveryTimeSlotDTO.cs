using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class DeliveryTimeSlotDTO
    {
        public int Id { get; set; }
        [Required]
        public System.TimeSpan StartTime { get; set; }
        [Required]
        public System.TimeSpan EndTime { get; set; }
        [Required]
        public int SlotCode { get; set; }
    }

    public class DeliveryTimeSlotModel
    {
        public int Id { get; set; }
        public string TimeSlot { get; set; }
        public int SlotCode { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class CancelOrderReturnModel
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public int Order_Id { get; set; }
        public int User_Id { get; set; }
        public double Mebr_Amount { get; set; }
    }
}
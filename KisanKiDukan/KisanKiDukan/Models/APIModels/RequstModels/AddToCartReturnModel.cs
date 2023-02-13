using KisanKiDukan.Models.APIModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.APIModels.RequstModels
{
    public class AddToCartReturnModel: MessageModel
    {
        public AllKartDetailModel Kart { get; set; }
    }
}
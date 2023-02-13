using KisanKiDukan.Models.APIModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.APIModels.RequstModels
{
    public class AllKartDetailModel
    {
        public int Kart_Id { get; set; }
        public List<CartDetailModel> KartDetail { get; set; }
    }
}
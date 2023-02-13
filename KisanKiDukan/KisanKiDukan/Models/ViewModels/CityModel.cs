using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.ViewModels
{
    public class CityModel
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public int State_Id { get; set; }
        public SelectList StateList { get; set; }
    }
}
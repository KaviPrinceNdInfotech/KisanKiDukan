﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class pbannershowmodel
    {
        public IEnumerable<PBannersModel> ProBanner { get; set; }
        public int? Page { get; set; }
        public int? NumberOfPages { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
    public class PBannersModel
    {
        public int Id { get; set; }
        public string promotionalbannerpath { get; set; }

    }
}
using KisanKiDukan.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class VendorListVm
    {
        public int Page { get; set; }
        public int NumberOfPages { get; set; }
        public string Term { get; set; }
        public IEnumerable<VendorDTO> Vendors { get; set; }
    }
}
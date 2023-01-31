using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class LocationListModel
    {
        public IEnumerable<LocationViewModel> Location { get; set; }
        public int? Page { get; set; }
        public int? NumberOfPages { get; set; }
    }
    public class LocationViewModel
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string TimeDay { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class CustomerListModel
    {
        public IEnumerable<CustomerModel> Customer { get; set; }
        public int? Page { get; set; }
        public int? NumberOfPages { get; set; }
    }
}
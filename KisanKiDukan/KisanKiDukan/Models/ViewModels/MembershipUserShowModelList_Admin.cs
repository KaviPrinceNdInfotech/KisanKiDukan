using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class MembershipUserShowModelList_Admin
    {
        public IEnumerable<MembershipUserShowModel_Admin> Member { get; set; }
        public int? Page { get; set; }
        public int? NumberOfPages { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class JoinMemberModelListAdmin
    {
        public IEnumerable<JoinMemberModelAdmin> Member { get; set; }
        public int? Page { get; set; }
        public int? NumberOfPages { get; set; }
    }
}
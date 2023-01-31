using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class MemberDetailAPIListModel
    {
        public IEnumerable<MemberDetailAPIModel> Members { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
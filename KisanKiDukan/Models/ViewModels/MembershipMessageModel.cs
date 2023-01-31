using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class MembershipMessageModel
    {
        public IEnumerable<MemberShipModel> MembCategory { get; set; }
        public int Prc { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
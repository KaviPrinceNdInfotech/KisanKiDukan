using KisanKiDukan.Models.APIModels.ResponseModels;
using KisanKiDukan.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class VarientVM
    {
        public int Product_Id { get; set; }
        public IEnumerable<PAvailDTO> VarientList { get; set; }
        public IEnumerable<ProductVarientReturn> VList { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public string ProductName { get; set; }
    }
}
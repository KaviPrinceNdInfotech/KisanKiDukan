using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.ViewModels
{
    public class CatWithProdVm
    {
        public IEnumerable<CatWithProdModel> Cateogries { get; set; }
    }
    public class CatWithProdModel
    {
        public int CatId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
        public List<ProdModel> Products { get; set; }
    }

    public class ProdModel
    {
        public int ProdId { get; set; }
        public string ProdName { get; set; }
    }
}
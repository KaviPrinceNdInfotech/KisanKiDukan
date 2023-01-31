using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KisanKiDukan.Models.DTO
{
    public class ADSubCategoryDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter subcategory name")]
        public string Name { get; set; }
        public string CatImage { get; set; }
        public Nullable<int> MainCat_Id { get; set; }
    }
}
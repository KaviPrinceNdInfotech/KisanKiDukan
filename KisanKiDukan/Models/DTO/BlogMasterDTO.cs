using KisanKiDukan.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KisanKiDukan.Models.DTO
{
    public class BlogMasterDTO
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        public string BlogImage { get; set; }
        [Required]
        public HttpPostedFileBase BlogFile { get; set; }
        [Required]
        public string Heading { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "Not More than 500 Words Are Allowed")]
        public string Short { get; set; }
        [Required, AllowHtml]
        public string Description { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public IEnumerable<Blogs> Blog { get; set; }

        public HttpPostedFileBase ImageBase { get; set; }
    }
    public class Blogs
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string BlogImage { get; set; }
        public HttpPostedFileBase BlogFile { get; set; }
        public string Heading { get; set; }
        public string Short { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Date { get; set; }

        public HttpPostedFileBase ImageBase { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KisanKiDukan.Models.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> ParentCategory { get; set; }
        public string CategoryImage { get; set; }
        public bool IsActive { get; set; }
        public string Page_Url { get; set; }
        public bool IsFeature { get; set; }
        public string UpToText { get; set; }
    }
}

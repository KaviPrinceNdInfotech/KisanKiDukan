
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
    
public partial class Blog
{

    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; }

    public string ImagePath { get; set; }

    public Nullable<int> BlogCategory_Id { get; set; }

    public string Author { get; set; }

    public Nullable<int> Product_Id { get; set; }

    public string Url { get; set; }

    public Nullable<System.DateTime> Publish { get; set; }

}

}

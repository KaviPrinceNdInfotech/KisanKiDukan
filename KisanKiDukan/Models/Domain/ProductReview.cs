
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
    
public partial class ProductReview
{

    public int Id { get; set; }

    public int Product_Id { get; set; }

    public string Tittle { get; set; }

    public System.DateTime ReviewDate { get; set; }

    public bool IsApproved { get; set; }

    public string Comment { get; set; }

    public Nullable<int> UserId { get; set; }

    public string AdminComment { get; set; }

}

}

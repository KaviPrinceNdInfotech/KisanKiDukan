
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
    
public partial class DiscountInfo
{

    public int Id { get; set; }

    public int ProductId { get; set; }

    public Nullable<int> DiscVal_Id { get; set; }

    public string Discount { get; set; }

    public string Description { get; set; }

    public string StartDate { get; set; }

    public string EndDate { get; set; }

    public Nullable<bool> Status { get; set; }

}

}

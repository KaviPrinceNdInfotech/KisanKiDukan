
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
    
public partial class Membership
{

    public int Id { get; set; }

    public double Amount { get; set; }

    public double Percentage { get; set; }

    public double BenifitAmount { get; set; }

    public int DistributionMonth { get; set; }

    public string Remark { get; set; }

    public int Customer_Id { get; set; }

    public System.DateTime CreateDate { get; set; }

    public System.DateTime UpdateDate { get; set; }

}

}

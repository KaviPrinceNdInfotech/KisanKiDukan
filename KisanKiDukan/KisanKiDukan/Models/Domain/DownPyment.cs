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
    
    public partial class DownPyment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MeritalStatus { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Nullable<int> PinCode { get; set; }
        public string PAN_Aadhar_Number { get; set; }
        public Nullable<decimal> MonthlyIncome { get; set; }
        public Nullable<int> product_Id { get; set; }
    }
}

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
    
    public partial class Customer
    {
        public int User_Id { get; set; }
        public string FullName { get; set; }
        public string Email_Id { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public Nullable<double> Mebr_Amount { get; set; }
        public Nullable<bool> IsPremiumMember { get; set; }
        public Nullable<System.DateTime> PremiumMemberOn { get; set; }
        public Nullable<int> otp { get; set; }
        public Nullable<int> Email_Otp { get; set; }
        public Nullable<bool> Isverify { get; set; }
    }
}
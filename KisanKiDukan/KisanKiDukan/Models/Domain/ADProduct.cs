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
    
    public partial class ADProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ADProduct()
        {
            this.ADProductDetails = new HashSet<ADProductDetail>();
        }
    
        public int Id { get; set; }
        public string ADTitle { get; set; }
        public string Description { get; set; }
        public Nullable<double> Price { get; set; }
        public string ADImage { get; set; }
        public Nullable<int> VendorId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ADProductDetail> ADProductDetails { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JhulayLal_Enterprise.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetCustomer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetCustomer()
        {
            this.AspNetSales = new HashSet<AspNetSale>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Contact_No { get; set; }
        public string CNIC { get; set; }
        public Nullable<int> AccountNo { get; set; }
        public byte[] Picture { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetSale> AspNetSales { get; set; }
    }
}

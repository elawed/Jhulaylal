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
    
    public partial class AspNetBill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetBill()
        {
            this.AspNetBillDetails = new HashSet<AspNetBillDetail>();
            this.AspNetSales = new HashSet<AspNetSale>();
        }
    
        public int Id { get; set; }
        public Nullable<int> FarmerID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Truck_No { get; set; }
        public Nullable<int> Bags_Qty { get; set; }
        public Nullable<int> Freight_Charges { get; set; }
        public Nullable<int> Commission { get; set; }
        public Nullable<int> Labour { get; set; }
        public Nullable<int> Market_Fees { get; set; }
        public Nullable<int> Telephone_Charges { get; set; }
        public Nullable<int> Accountant { get; set; }
        public Nullable<int> Total_Expenses { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<int> Expenses { get; set; }
        public Nullable<int> Total_Amount { get; set; }
        public string Bill_Status { get; set; }
    
        public virtual AspNetFarmer AspNetFarmer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetBillDetail> AspNetBillDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetSale> AspNetSales { get; set; }
    }
}
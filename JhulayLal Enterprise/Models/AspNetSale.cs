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
    
    public partial class AspNetSale
    {
        public int Id { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Particular { get; set; }
        public Nullable<int> BillID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<int> Received { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<int> Remaining { get; set; }
    
        public virtual AspNetBill AspNetBill { get; set; }
        public virtual AspNetCustomer AspNetCustomer { get; set; }
    }
}
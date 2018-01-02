using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JhulayLal_Enterprise.Models
{
    public class Sale
    {
        public string CustomerName { get; set; }
        public string Particular { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<int> Received { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<int> Remaining { get; set; }
        public string truck { get; set; }

    }
}
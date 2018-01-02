using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JhulayLal_Enterprise.Models
{
    public class FarmerSalesModel
    {
        public string Name { get; set; }

        public List<Sale> saleslist { get; set; }
       
    }
}
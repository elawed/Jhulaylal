using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JhulayLal_Enterprise.Models
{
    public class Farmer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Contact_No { get; set; }
        public string Imagepath { get; set; }

        public HttpPostedFileBase image { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class AdminAssociation
    {
        public int region { get; set; }
        public int area { get; set; }
        public string assName { get; set; }
        public int memberIdx { get; set; }
        public string assType { get; set; }

        public int assIdx { get; set; }
        public string memo { get; set; }
    }
}
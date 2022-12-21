using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class ClassCompare
    {
        public int assType { get; set; }
        public int assIdx { get; set; }
        public Check[] checkList { get; set; }
        public class Check
        {
            public int clubIdx { get; set; }
            public int memberIdx { get; set; }
            public string classCode { get; set; }
        }
    }
}
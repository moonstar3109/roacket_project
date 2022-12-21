using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class ExcelMemberAdd
    {   
        public int assType { get; set; }
        public Memberlist[] memberList { get; set; }
        public class Memberlist
        {
            public int clubIdx { get; set; }
            public string memberName { get; set; }
            public string gender { get; set; }
            public string birth { get; set; }
            public string phone { get; set; }
            public string classCode { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class AdminMember
    {
        public int memberIdx { get; set; }
        public int assIdx { get; set; }
        public int regionCode { get; set; }
        public AddMember[] memberList { get; set; }

        public class AddMember
        {
            public int areaCode { get; set; }
            public int clubIdx { get; set; }
            public string memberName { get; set; }
            public string gender { get; set; }
            public string birth { get; set; }
            public string phone { get; set; }
            public int regionClassCode { get; set; }
            public int areaClassCode { get; set; }

        }
    }
}
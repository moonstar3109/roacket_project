using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class MemberClassAuth
    {
        public int assIdx { get; set; }
        public MbClass[] classList { get; set; }
        public class MbClass
        {
            public int clubIdx { get; set; }
            public int memberIdx { get; set; }
        }
    }
}
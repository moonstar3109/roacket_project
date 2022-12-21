using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class AssClub
    {
        public int assIdx { get; set; }
        public int clubIdx { get; set; }

        public Regist[] registList { get; set; }

        public class Regist
        {
            public int clubIdx { get; set; }
        }
    }
}
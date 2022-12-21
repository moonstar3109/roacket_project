using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class MembersClasses
    {
        public RegionClass[] regionClasses { get; set; }

        public class RegionClass
        {
            public int memberIdx { get; set; }
            public int regionClassCode { get; set; }
        }

        public AreaClass[] areaClasses { get; set; }

        public class AreaClass
        {
            public int memberIdx { get; set; }
            public int areaClassCode { get; set; }
        }
    }
}
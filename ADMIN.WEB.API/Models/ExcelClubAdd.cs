using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class ExcelClubAdd
    {
        public int assType { get; set; }
        public int assIdx { get; set; }
        public ClubList[] clubList { get; set; }
        public class ClubList
        {
            public string clubName { get; set; }
            public string region { get; set; }
            public string area { get; set; }
        }
    }
}
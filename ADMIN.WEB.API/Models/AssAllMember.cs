using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class AssAllMember
    {
        public int memberIdx { get; set; }
        public int clubIdx { get; set; }
        public string birth { get; set; }
        public string phone { get; set; }
        public string gender { get; set; }
        public int assIdx { get; set; }
        public int regionClassCode { get; set; }
        public int areaClassCode { get; set; }
        public string reason { get; set; }
        public int dressCode { get; set; }
        public int shoesCode { get; set; }
    }
}
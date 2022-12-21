using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class Transfer
    {
        public int memberIdx { get; set; }
        public int befRegionCode { get; set; }
        public int befAreaCode { get; set; }
        public int clubIdx { get; set; }
        public int aftRegionCode { get; set; }
        public int aftAreaCode { get; set; }
        public int tClubIdx { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public int transIdx { get; set; }
        public int isOk { get; set; }
        public string editReason { get; set; }
    }
}
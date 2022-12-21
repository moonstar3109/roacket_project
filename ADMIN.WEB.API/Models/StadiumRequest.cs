using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class StadiumRequest
    {
        public int requestIdx { get; set; }
        public int regionCode { get; set; }
        public int areaCode { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public string stadiumName { get; set; }
        public string stadiumAddr { get; set; }
        public string stadiumAddr2 { get; set; }
        public string phone { get; set; }
        public string searchList { get; set; }
        public int isUse { get; set; }
    }
}
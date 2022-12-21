using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class Expulsion
    {
        public string type { get; set; }
        public int idx { get; set; }
        public int memberIdx { get; set; }
        public string expReason { get; set; }
        public DateTime srtDate { get; set; }
        public DateTime endDate { get; set; }
        public Boolean isExp { get; set; }

        public int expIdx { get; set; }
    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class AssTransfer
    {
        public int transIdx { get; set; }
        public string cancelReason { get; set; }
        public int assIdx { get; set; }

    }
}
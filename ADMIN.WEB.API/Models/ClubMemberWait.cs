using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class ClubMemberWait
    {
        public int waitIdx { get; set; }
        public int optionCode { get; set; }
        public string rejectReason { get; set; }
    }
}
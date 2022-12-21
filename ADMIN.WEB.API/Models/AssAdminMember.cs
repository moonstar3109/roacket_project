using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class AssAdminMember
    {
        public int assIdx { get; set; }
        public int memberIdx { get; set; }
        public int poCode { get; set; }

        public DeleteMember[] delList { get; set; }

        public class DeleteMember
        {
            public int memberIdx { get; set; }
        }
    }
}
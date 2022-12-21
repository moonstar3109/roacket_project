using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class AssAdminPosition
    {   
        public int assIdx { get; set; }
        public int poCode { get; set; }
        public string position { get; set; }
        public int assManage { get; set; }
        public int noticeManage { get; set; }
        public int clubManage { get; set; }
        public int memberManage { get; set; }
        public int transferManage { get; set; }

        public PositionList[] positions { get; set; }

        public class PositionList
        {
            public string position { get; set; }
        }
    }
}
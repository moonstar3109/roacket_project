using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class EditMemberClass
    {
        public int assIdx { get; set; }
        public int assType { get; set; }
        public string reason { get; set; }
        public EditMemberClasses[] classesList { get; set; }
        public class EditMemberClasses
        {
            public int clubIdx { get; set; }
            public int memberIdx { get; set; }
            public int classCode { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class AssociationSetting
    {
        public int assIdx { get; set; }
        public string assName { get; set; }
        public string assType { get; set; }
        public int memberIdx { get; set; }
        public int regionCode { get; set; }
        public int areaCode { get; set; }
    }
}
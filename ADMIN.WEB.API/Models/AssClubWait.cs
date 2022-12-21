using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class AssClubWait
    {

        public int assIdx { get; set; }
        public int clubIdx { get; set; }
        public int isOk { get; set; }
        public string reason { get; set; }
    }
}
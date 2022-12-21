using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class ClubName
    {
        public int clubIdx { get; set; }
        public string befClubName { get; set; }
        public string aftClubName { get; set; }

        public int editIdx { get; set; }
        public int isOk { get; set; }

    }
}
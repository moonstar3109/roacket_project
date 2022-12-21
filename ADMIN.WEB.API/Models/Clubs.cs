using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class Clubs
    {
        public int clubIdx { get; set; }
        public string clubName { get; set; }
        public int stadiumIdx { get; set; }
        public string memo { get; set; }
        public ClubImage[] images { get; set; }

        public class ClubImage
        {   
            public string fileName { get; set; }
            public string filePath { get; set; }
        }
    }
}
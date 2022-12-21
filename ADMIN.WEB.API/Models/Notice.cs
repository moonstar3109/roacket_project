using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class Notice
    {   
        public int assIdx { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public int noticeIdx { get; set; }
        public int isAll { get; set; }
        public int isRegion { get; set; }
        public int isArea { get; set; }
        public int isClub { get; set; }

        public File[] files { get; set; }
        public File[] editFiles { get; set; }

        public class File
        {
            public int no { get; set; }
            public string fileName { get; set; }
            public string filePath { get; set; }
        }


     
        
    }


}
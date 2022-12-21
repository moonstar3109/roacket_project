using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class Classes
    {
        public int assIdx { get; set; }

        public EditClass[] edits { get; set; }
        public AreaList[] areas { get; set; }
        public ClassList[] cls { get; set; }

        public class AreaList 
        { 
            public int area { get; set; }
        }

        public class ClassList
        {
            public string stClass { get; set; }
            public string className { get; set; } 
        }

        public class EditClass
        {
            public string stClass { get; set; }
            public int classCode { get; set; }
            public string className { get; set; }
        }
    }
}
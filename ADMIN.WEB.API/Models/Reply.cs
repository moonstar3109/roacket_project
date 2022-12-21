using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class Reply
    {
        public int noticeIdx { get; set; }
        public int repIdx { get; set; }
        public int depth { get; set; }
        public string contents { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public int writer { get; set; }
        //public EditReply[] editReply { get; set; }

        //public class EditReply
        //{

        //    public string fileName { get; set; }
        //    public string filePath { get; set; }
        //}
    }

   
}
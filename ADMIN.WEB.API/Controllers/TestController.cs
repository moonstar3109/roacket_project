using ADMIN.WEB.API.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class TestController : ApiController
    {
        public string imgDomain = ConfigurationManager.AppSettings.Get("imgDomain");
        public object Get(string filePath)
        {
            string path = HttpContext.Current.Server.MapPath("~");

            string sourcePath = path + filePath.Replace(imgDomain + "/", "").Replace("/", "\\");

            FileStream fs = new FileStream(sourcePath, FileMode.Open);
            BinaryReader sr = new BinaryReader(fs);

            // read byte
            byte[] readBytes = sr.ReadBytes((int)fs.Length);

            string byteSum = Convert.ToBase64String(readBytes);
            
            return byteSum;
        }
    }
}

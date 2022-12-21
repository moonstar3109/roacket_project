using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Common
{
    public class Images
    {
        public string saveImage(string type, byte[] bytetest, string extention)
        {

            var directory = string.Empty;

            //filePath 로 업로드 시킬 폴더 분기
            switch (type)
            {
                case "profile":
                    directory = "/upload/profile/";
                    break;
                case "image":
                    directory = "/upload/image/";
                    break;
                case "":

                    break;
            }
            string nm = Path.GetRandomFileName();
            nm = nm.Replace(".", "") + "." + extention;
            string filePath = directory + nm;
            var root = HttpContext.Current.Server.MapPath(filePath);

            try
            {
                using (var ms = new MemoryStream(bytetest))
                {
                    using (var fs = new FileStream(root, FileMode.Create))
                    {
                        ms.WriteTo(fs);
                    }
                }
            }
            catch (Exception exp)
            {

                return exp.ToString();
            }
            return filePath;
        }
    }
}
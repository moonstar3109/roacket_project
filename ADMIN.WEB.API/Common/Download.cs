using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Common
{
    public class Download
    {
        public string imgDomain = ConfigurationManager.AppSettings.Get("imgDomain");

        public string GetFile(string filePath)
        {
            string sourcePath = filePath;

            //string sourcePath = path + filePath.Replace(imgDomain+"/", "").Replace("/","\\");
            byte[] readBytes = null;
            using (FileStream fs = new FileStream(sourcePath, FileMode.Open))
            {
                BinaryReader sr = new BinaryReader(fs);

                // read byte
                readBytes = sr.ReadBytes((int)fs.Length);
            }

            string byteSum = Convert.ToBase64String(readBytes);

            return byteSum;
        }

    }
}
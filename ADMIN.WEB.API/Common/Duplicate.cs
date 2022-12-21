using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Common
{
    public class Duplicate
    {
        //중복파일명처리
        public string FileUploadName(String dirPath, String fileN)
        {
            string fileName = fileN;

            if (fileN.Length > 0)
            {
                int indexOfDot = fileName.LastIndexOf(".");
                string strName = fileName.Substring(0, indexOfDot);
                string strExt = fileName.Substring(indexOfDot);

                bool bExist = true;
                int fileCount = 0;

                string dirMapPath = string.Empty;

                while (bExist)
                {
                    dirMapPath = dirPath;
                    string pathCombine = System.IO.Path.Combine(dirMapPath, fileName);

                    if (System.IO.File.Exists(pathCombine))
                    {
                        fileCount++;
                        fileName = strName + "(" + fileCount + ")" + strExt;
                    }
                    else
                    {
                        bExist = false;
                    }
                }
            }

            return fileName;

        }


        //중복폴더명 처리
        public string MakeFolderName(string dirPath, string folderName)
        {
            bool bExist = true;
            int fileCount = 0;

            string dirMapPath = string.Empty;

            while (bExist)
            {
                dirMapPath = dirPath;
                string pathCombine = System.IO.Path.Combine(dirMapPath, folderName);
                DirectoryInfo di = new DirectoryInfo(pathCombine);
                if(di.Exists != true)
                {
                    bExist = false;
                }
                //if (di.Exists == true)
                //{
                //    fileCount++;
                //    folderName = folderName.Split('(')[0];
                //    folderName = folderName + "(" + fileCount + ")";
                //}
                //else
                //{
                //    bExist = false;
                //}
            }
            return folderName;
        }
    }
}
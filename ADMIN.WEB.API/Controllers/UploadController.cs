using ADMIN.WEB.API.Common;
using ADMIN.WEB.API.Jwt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class UploadController : ApiController
    {
        public string imgDomain = ConfigurationManager.AppSettings.Get("imgDomain");
        public static string jToken = string.Empty;
        /// <remarks>
        /// 파일업로드 입니다.
        /// </remarks>
        /// <returns></returns>
        [Route("Upload")]
        public object Post()
        {
            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();
            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
            
            List<object> list = new List<object>();
            var fileName = string.Empty;
            //string folderName = string.Empty;

            string userFolder = "user_" + memberIdx.ToString();
            string temp = string.Empty;
            string path = string.Empty;
            int count = 0;

            var dp = new Duplicate();

            DataTable dt = new DataTable();
            dt.Columns.Add("fileName", typeof(string));
            dt.Columns.Add("filePath", typeof(string));

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                for(int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    var file = HttpContext.Current.Request.Files[i];
                    string type = HttpContext.Current.Request.Files.AllKeys[i].ToString();
                    if(type == "file")
                    {
                        
                        try
                        {
                            temp = HttpContext.Current.Server.MapPath("~/uploads/temp_file/").ToString();

                            if (count == 0)
                            {
                                //userFolder = dp.MakeFolderName(temp, userFolder);
                                DirectoryInfo di = new DirectoryInfo(temp + userFolder);
                                if (!di.Exists)
                                {
                                    di.Create();
                                }
                            }

                            dt.Rows.Add();

                            fileName = Path.GetFileNameWithoutExtension(file.FileName).Normalize()+Path.GetExtension(file.FileName);
                            dt.Rows[i]["fileName"] = fileName;
                            path = HttpContext.Current.Server.MapPath("~/uploads/temp_file/" + userFolder).ToString();
                            fileName = dp.FileUploadName(path, fileName);

                            var filePath = Path.Combine(
                                path,
                                fileName
                            );

                            file.SaveAs(filePath);

                            dt.Rows[i]["filePath"] = "/uploads/temp_file/" + userFolder + "/";
                        }
                        catch (Exception e)
                        {
                            dic["success"] = false;
                            dic["message"] = e.ToString();
                        }
                    }
                    else
                    {
                        //image
                        try
                        {
                            temp = HttpContext.Current.Server.MapPath("~/uploads/temp_image/").ToString();
                            if (count == 0)
                            {
                                //userFolder = dp.MakeFolderName(temp, userFolder);
                                DirectoryInfo di = new DirectoryInfo(temp + userFolder);
                                if (!di.Exists)
                                {
                                    di.Create();
                                }
                            }

                            dt.Rows.Add();

                            fileName = Path.GetFileNameWithoutExtension(file.FileName).Normalize() + Path.GetExtension(file.FileName);
                            dt.Rows[i]["fileName"] = fileName;
                            path = HttpContext.Current.Server.MapPath("~/uploads/temp_image/" + userFolder).ToString();
                            fileName = dp.FileUploadName(path, fileName);

                            var filePath = Path.Combine(
                                path,
                                fileName
                            );

                            file.SaveAs(filePath);

                            dt.Rows[i]["filePath"] = "/uploads/temp_image/" + userFolder + "/";
                        }
                        catch (Exception e)
                        {
                            dic["success"] = false;
                            dic["message"] = e.ToString();
                        }
                    }

                    count++;
                }
            }
            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            dic["data"] = dt;

            return dic;
        }

  
    }
}

using ADMIN.API.DA;
using ADMIN.WEB.API.Common;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ClubController : ApiController
    {
        public static string jToken = string.Empty;
        public string imgDomain = ConfigurationManager.AppSettings.Get("imgDomain");
        /// <remarks>
        /// 클럽상세정보
        /// 
        ///     {
        ///         "clubIdx" : 2,
        ///         "regionCode" : 1,
        ///         "areaCode" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Club")]
        public object Get(string search)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();
            if(Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }
            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();

            JObject root = JObject.Parse(search);

            int regionCode = Convert.ToInt32(root["regionCode"].ToString());
            int areaCode = Convert.ToInt32(root["areaCode"].ToString());
            int clubIdx = Convert.ToInt32(root["clubIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataTable dt = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetClubList(clubIdx,regionCode, areaCode);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if (dt.Rows.Count == 0)
            {
                dic["success"] = false;
                dic["data"] = new List<object>();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt;

            return dic;
        }


        /// <remarks>
        /// 클럽상세정보
        /// 
        ///     {
        ///         "id" : 1384
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Club/{id}")]
        public object Get(int id)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }
            ////token 정보 조회
            //jToken = Request.Headers.Authorization.ToString();

            //int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataSet ds = null;
            DataTable folderDt = null;
            string folderName = string.Empty;
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetClub(id);
                    folderDt = biz.GetFolderName("image", "club", id);
                    folderName = folderDt.Rows[0]["filePath"].ToString();
                    folderName = folderName.Replace("https://adm-images.roacket.com/uploads/image/", "");
                    folderName = folderName.Split('/')[0];
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];

            dt.Columns.Add("image", typeof(DataTable));

            dt.Rows[0]["image"] = dt2; 
            if(dt2.Rows.Count != 0)
            {
                dt2.Columns.Add("imgSrc", typeof(string));
                dt2.Columns.Add("fileName", typeof(string));

                foreach(DataRow row in dt2.Rows)
                {
                    string imgSrc = row["filePath"].ToString();
                    row["imgSrc"] = imgSrc;
                    row["fileName"] = imgSrc.Replace(imgDomain,"").Replace("/uploads/image/"+folderName+"/", "");
                    row["filePath"] = imgSrc.Replace(imgDomain, "").Replace(row["fileName"].ToString(), "");
                }
            }
            dic["success"] = true;
            dic["data"] = dt;

            return dic;

        }


        /// <remarks>
        /// 클럽정보수정 
        /// 
        ///     {       
        ///           "clubIdx" : 1,
        ///           "clubName": "클럽이름",
        ///           "stadiumIdx": 1,
        ///           "memo": "클럽설명",
        ///           "images": [
        ///             {   
        ///                 "fileName": "images.jpg",
        ///                 "filePath": "/uploads/temp_image/"
        ///             },
        ///             {
        ///                 "fileName": "dddddd.png",
        ///                 "filePath": "/uploads/temp_image/"
        ///             }
        ///            ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Club")]
        public object Put([FromBody] Clubs c)
        {

            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }
            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();

            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int result = 0;

            string path = string.Empty;
            string fileName = string.Empty;
            string sourcePath = string.Empty;
            string type = string.Empty;

            var dp = new Duplicate();

            DataTable images = new DataTable();
            images.Columns.Add("fileName", typeof(string));
            images.Columns.Add("filePath", typeof(string));

            int count = 0;
            DataTable folderDt = null;
            string folderName = "user_" + memberIdx.ToString();
            int forCount = c.images.Length;
            try
             {
                using (var biz = new Api_Biz()) 
                {
                    //수정이 있는 경우
                    if(c.images.Length > 0)
                    {
                      
                        //이미지 수정
                        for (int i = 0; i < forCount; i++)
                        {
                            if (count == 0)
                            {
                                try
                                {
                                    using (var fnGetName = new Api_Biz())
                                    {
                                        folderDt = fnGetName.GetFolderName("image", "club", c.clubIdx);
                                    }
                                }
                                catch (Exception f)
                                {

                                    throw;
                                }


                            }
                            if (folderDt.Rows.Count > 0)
                            {
                                folderName = folderDt.Rows[0]["filePath"].ToString();
                                folderName = folderName.Replace("https://adm-images.roacket.com/uploads/image/", "");
                                folderName = folderName.Split('/')[0];
                            }
                            else
                            {
                                //없는경우 폴더 생성
                                if(count == 0)
                                {
                                    path = HttpContext.Current.Server.MapPath("~/uploads/image/");
                                    folderName = dp.MakeFolderName(path, folderName);
                                    DirectoryInfo di = new DirectoryInfo(path + folderName);
                                    di.Create();
                                }

                            }
                            
                           
                            fileName = c.images[i].fileName;
                            sourcePath = HttpContext.Current.Server.MapPath(c.images[i].filePath.Replace("/", "\\") + c.images[i].fileName);

                            path = HttpContext.Current.Server.MapPath("~/uploads/image/" + folderName);
                            fileName = dp.FileUploadName(path, fileName);

                            path = path +"/"+ fileName;
                            //파일이동
                            File.Copy(sourcePath, path);

                            path = imgDomain + "/uploads/image/" + folderName + "/" + fileName;


                            DataRow row = images.NewRow();
                            row["fileName"] = fileName;
                            row["filePath"] = path;

                            images.Rows.InsertAt(row, i);

                            count++;
                        } //end for

                        type = "club";
                        //이미지 수정 처리
                        result = biz.EditUpload(type, c.clubIdx, images);
                        if(result != 0)
                        {
                            dic["success"] = false;
                            dic["message"] = "database error";
                            return dic;
                        }
                    }
                    result = biz.EditClub(c.clubIdx, memberIdx, c.clubName, c.stadiumIdx, c.memo);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = "처리중 오류";
                return dic;
            }


            if (result == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상처리";

            }
            else
            {
                dic["success"] = false;
                dic["message"] = "처리중 오류";
            }

            return dic;
        }

    }

    
    
}

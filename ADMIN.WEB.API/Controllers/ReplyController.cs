using ADMIN.API.DA;
using ADMIN.WEB.API.Common;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Statnco.FW.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ReplyController : ApiController
    {
        public static string jToken = string.Empty;
        public string imgDomain = new GlobalValue().imgDomain.ToString();

        /// <remarks>
        /// 댓글 조회
        /// 
        ///     {
        ///        "공지글Idx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Reply/{id}")]
        public object Get(int id)
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

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            string fileName = string.Empty;
            string filePath = string.Empty;
            

            DataSet ds = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetReplyList(id);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }
            if(ds.Tables[0].Rows.Count > 0)
            {
                DataTable depth0 = ds.Tables[0];
                depth0.Columns.Add("fileName", typeof(string));
                depth0.Columns.Add("imgSrc", typeof(string));
                for (int i = 0; i < depth0.Rows.Count; i++)
                {
                    if (depth0.Rows[i]["filePath"].ToString() == "")
                    {
                        depth0.Rows[i]["imgSrc"] = string.Empty;
                        depth0.Rows[i]["filePath"] = string.Empty;
                        depth0.Rows[i]["fileName"] = string.Empty;

                    }
                    else
                    {
                        depth0.Rows[i]["imgSrc"] = depth0.Rows[i]["filePath"];
                        filePath = depth0.Rows[i]["filePath"].ToString().Replace(imgDomain, "");
                        fileName = filePath.Replace("/uploads/", "");
                        fileName = fileName.Substring(fileName.IndexOf("/") + 1);
                        depth0.Rows[i]["filePath"] = filePath.Replace(fileName, "");
                        depth0.Rows[i]["fileName"] = fileName;
                    }
                }

                DataTable depth1 = ds.Tables[1];
                depth1.Columns.Add("fileName", typeof(string));
                depth1.Columns.Add("imgSrc", typeof(string));
                for (int i = 0; i < depth1.Rows.Count; i++)
                {
                    if (depth1.Rows[i]["filePath"].ToString() == "")
                    {
                        depth1.Rows[i]["imgSrc"] = string.Empty;
                        depth1.Rows[i]["filePath"] = string.Empty;
                        depth1.Rows[i]["fileName"] = string.Empty;

                    }
                    else
                    {
                        depth1.Rows[i]["imgSrc"] = depth1.Rows[i]["filePath"];
                        filePath = depth1.Rows[i]["filePath"].ToString().Replace(imgDomain, "");
                        fileName = filePath.Replace("/uploads/", "");
                        fileName = fileName.Substring(fileName.IndexOf("/") + 1);
                        depth1.Rows[i]["filePath"] = filePath.Replace(fileName, "");
                        depth1.Rows[i]["fileName"] = fileName;
                    }
                }

                DataTable depth2 = ds.Tables[2];
                depth2.Columns.Add("fileName", typeof(string));
                depth2.Columns.Add("imgSrc", typeof(string));
                for (int i = 0; i < depth2.Rows.Count; i++)
                {
                    if (depth2.Rows[i]["filePath"].ToString() == "")
                    {
                        depth2.Rows[i]["imgSrc"] = string.Empty;
                        depth2.Rows[i]["filePath"] = string.Empty;
                        depth2.Rows[i]["fileName"] = string.Empty;

                    }
                    else
                    {
                        depth2.Rows[i]["imgSrc"] = depth2.Rows[i]["filePath"];
                        filePath = depth2.Rows[i]["filePath"].ToString().Replace(imgDomain, "");
                        fileName = filePath.Replace("/uploads/", "");
                        fileName = fileName.Substring(fileName.IndexOf("/") + 1);
                        depth2.Rows[i]["filePath"] = filePath.Replace(fileName, "");
                        depth2.Rows[i]["fileName"] = fileName;
                    }
                }

                DataTable depth3 = ds.Tables[3];
                depth3.Columns.Add("fileName", typeof(string));
                depth3.Columns.Add("imgSrc", typeof(string));
                for (int i = 0; i < depth3.Rows.Count; i++)
                {
                    if (depth3.Rows[i]["filePath"].ToString() == "")
                    {
                        depth3.Rows[i]["imgSrc"] = string.Empty;
                        depth3.Rows[i]["filePath"] = string.Empty;
                        depth3.Rows[i]["fileName"] = string.Empty;

                    }
                    else
                    {
                        depth3.Rows[i]["imgSrc"] = depth3.Rows[i]["filePath"];
                        filePath = depth3.Rows[i]["filePath"].ToString().Replace(imgDomain, "");
                        fileName = filePath.Replace("/uploads/", "");
                        fileName = fileName.Substring(fileName.IndexOf("/") + 1);
                        depth3.Rows[i]["filePath"] = filePath.Replace(fileName, "");
                        depth3.Rows[i]["fileName"] = fileName;
                    }
                }
                DataTable replyCnt = ds.Tables[4];

                depth0.Columns.Add("replys", typeof(List<object>));
                depth1.Columns.Add("replys", typeof(List<object>));
                depth2.Columns.Add("replys", typeof(List<object>));

                //depth2 
                for(int i = 0; i < depth2.Rows.Count; i++)
                {
                    int repIdx = Convert.ToInt32(depth2.Rows[i]["repIdx"].ToString());
                    List<object> list = new List<object>();
                    for (int ii = 0; ii < depth3.Rows.Count; ii++)
                    {
                        if( repIdx == Convert.ToInt32(depth3.Rows[ii]["preRepIdx"].ToString()) )
                        {
                            list.Add(depth3.Rows[ii].DataRowToJson());
                            depth2.Rows[i]["replys"] = list;
                        }

                    }
                }

                //depth1
                for (int i = 0; i < depth1.Rows.Count; i++)
                {
                    int repIdx = Convert.ToInt32(depth1.Rows[i]["repIdx"].ToString());
                    List<object> list = new List<object>();
                    for (int ii = 0; ii < depth2.Rows.Count; ii++)
                    {
                        if (repIdx == Convert.ToInt32(depth2.Rows[ii]["preRepIdx"].ToString()))
                        {
                            list.Add(depth2.Rows[ii].DataRowToJson());
                            depth1.Rows[i]["replys"] = list;
                        }

                    }
                }

                //depth0
                for (int i = 0; i < depth0.Rows.Count; i++)
                {
                    int repIdx = Convert.ToInt32(depth0.Rows[i]["repIdx"].ToString());
                    List<object> list = new List<object>();
                    for (int ii = 0; ii < depth1.Rows.Count; ii++)
                    {
                        if (repIdx == Convert.ToInt32(depth1.Rows[ii]["preRepIdx"].ToString()))
                        {
                            list.Add(depth1.Rows[ii].DataRowToJson());
                            depth0.Rows[i]["replys"] = list;
                        }

                    }
                }

                dic["success"] = true;
                dic["replys"] = depth0;
                dic["replyCnt"] = replyCnt.Rows[0]["replyCnt"];
                return dic;
            }
            else
            {
                dic["success"] = false;
                dic["replys"] = new DataTable();
                dic["replyCnt"] = 0;
                return dic;
            }

        }

        /// <remarks>
        /// 댓글 작성
        /// 
        ///     {
        ///        "noticeIdx" : 1,
        ///        "repIdx" : 0,
        ///        "depth" : 0,
        ///        "contents" : "어쩌구저쩌구",
        ///        "fileName" : "",
        ///        "filePath" : ""
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Reply")]
        public object Post([FromBody] Reply rp)
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

            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0;
            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());  
            string path = string.Empty;
            string fileName = string.Empty;
            string userFolder = "user_" + memberIdx.ToString();

            var dp = new Duplicate();

            if (rp.fileName != "")
            {
                string sourcePath = HttpContext.Current.Server.MapPath(rp.filePath.Replace("/", "\\") + rp.fileName);
                path = HttpContext.Current.Server.MapPath("~/uploads/image/");

                
                DirectoryInfo di = new DirectoryInfo(path + userFolder);
                if (!di.Exists) 
                { 
                    di.Create();
                }

                path = path + "/" + userFolder;

                fileName = dp.FileUploadName(path, rp.fileName);

                path = path + "/" + fileName;

                //파일이동
                File.Copy(sourcePath, path);
                
                //댓글만 통짜경로
                path = imgDomain + "/uploads/image/"+ userFolder + "/" + fileName;
            }

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.SetReply(memberIdx, rp.noticeIdx, rp.repIdx, rp.depth, rp.contents, path);
                }
            }
            catch (Exception exp)
            {

                dic["success"] = false;
                dic["message"] = exp.ToString();
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

        /// <remarks>
        /// 댓글 수정
        /// 
        ///     {
        ///        "noticeIdx" : 1,
        ///        "writer" : 1,
        ///        "repIdx" : 0,
        ///        "contents" : "어쩌구저쩌구",
        ///        "fileName" : "",
        ///        "filePath" : ""
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Reply")]
        public object Put([FromBody] Reply rp)
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

            Dictionary<string, object> dic = new Dictionary<string, object>();

            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            if (rp.writer != memberIdx)
            {
                dic["success"] = false;
                dic["message"] = "permission denied";
                return dic;
            }

            int result = 0;

            //사진 업로드
            string path = string.Empty;
            string fileName = string.Empty;
            string sourcePath = string.Empty;
            string type = string.Empty;

            string userFolder = "user_" + memberIdx.ToString();

            string editFile = rp.filePath.Replace("/uploads/", "").Split('/')[0];


            Duplicate dp = new Duplicate();
            //파일삭제
            if (rp.fileName == string.Empty)
            {
                path = string.Empty;
            }   
            else
            {
                if (editFile == "temp_image")
                {
                    //rp.filePath = /uploads/temp_image/
                    //파일수정
                    sourcePath = HttpContext.Current.Server.MapPath(rp.filePath.Replace("/", "\\") + rp.fileName);
                    path = HttpContext.Current.Server.MapPath("~/uploads/image/");

                    DirectoryInfo di2 = new DirectoryInfo(path + userFolder);

                    if (!di2.Exists)
                    {
                        di2.Create();
                    }

                    path = path + userFolder;

                    fileName = dp.FileUploadName(path, rp.fileName);

                    path = path + "/" + fileName;

                    //파일이동
                    File.Copy(sourcePath, path);

                    path = imgDomain + "/uploads/image/" + userFolder + "/" + fileName;
                }
                else
                {
                    //기존파일
                    path = imgDomain + rp.filePath + rp.fileName;
                }
            }
            try
            {
                using(var biz = new Api_Biz())
                {
                    result = biz.EditReply(memberIdx, rp.noticeIdx, rp.repIdx, rp.contents, path);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
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

        /// <remarks>
        /// 댓글 삭제
        /// 
        ///     {
        ///        "repIdx" : 1,
        ///     }
        /// </remarks>
        /// <returns></returns>
        [Route("Reply")]
        public object delete([FromBody] DelReply rp)
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

            Dictionary<string, object> dic = new Dictionary<string, object>();

            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            if (rp.writer != memberIdx)
            {
                dic["success"] = false;
                dic["message"] = "permission denied";
                return dic;
            }

            int result = 0;

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.DelReply(rp.repIdx, memberIdx);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
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

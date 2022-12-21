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
    public class TransferController : ApiController
    {
        public string jToken = string.Empty;
        public string imgDomain = ConfigurationManager.AppSettings.Get("imgDomain");

        /// <remarks>
        /// 이적리스트
        /// 
        ///     {
        ///         "clubIdx" : 1,
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Transfer")]
        public object Get(string search)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            DataTable dt = null;

            JObject root = JObject.Parse(search);
            int id = Convert.ToInt32(root["clubIdx"].ToString());
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetTransferList(id);
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
                dic["success"] = true;
                dic["data"] = new DataTable();
                return dic;
            }
            dic["success"] = true;
            dic["data"] = dt;
            return dic;

        }

        /// <remarks>
        /// 이적상세조회
        /// 
        ///     {
        ///         "transIdx" : 1,
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Transfer/{id}")]
        public object Get(int id)
        {
            //Dictionary<string, object> check = new Dictionary<string, object>();

            //if (Request.Headers.Authorization == null)
            //{
            //    check["success"] = false;
            //    check["message"] = "토큰이 없습니다";
            //    return check;
            //}

            DataSet ds = null;
            DataTable dt1 = null;

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetTransferDetail(id);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            dt1 = ds.Tables[0];
            dt1.Columns.Add("editDetail", typeof(DataTable));
            dt1.Rows[0]["editDetail"] = new DataTable();

            if(Convert.ToInt32(ds.Tables[0].Rows[0]["isOk"].ToString()) != 0)
            {
                dt1.Rows[0]["editDetail"] = ds.Tables[1];
            }

            if(dt1.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["data"] = new DataTable();
            }

            dic["success"] = true;
            dic["data"] = dt1;

            return dic;
            
        }

        /// <remarks>
        /// 이적신청
        /// 
        ///     {
        ///         "memberIdx":1,
        ///         "befRegionCode" : 1,
        ///         "befAreaCode" : 2,
        ///         "clubIdx" : 1,
        ///         "aftRegionCode" : 2,
        ///         "aftAreaCode" : 3,
        ///         "tClubIdx" : 2,
        ///         "fileName" : "이적신청서.jpg",
        ///         "filePath" : "/uploads/temp_image/",
        ///         "reason" : "사유사유"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Transfer")]
        public object Post([FromBody] Transfer tf)
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

            int reqMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0;

            string fileName = string.Empty;
            string sourcePath = string.Empty;
            string type = string.Empty;
            string path = string.Empty;

            var dp = new Duplicate();

            DataTable files = new DataTable();
            files.Columns.Add("fileName", typeof(string));
            files.Columns.Add("filePath", typeof(string));

            //파일 업로드 
            if (tf.fileName != "")
            {
                fileName = string.Empty;
                sourcePath = HttpContext.Current.Server.MapPath(tf.filePath.Replace("/", "\\") + tf.fileName);

                type = tf.filePath.Substring(tf.filePath.IndexOf("_") + 1).ToString();
                path = HttpContext.Current.Server.MapPath("\\uploads\\" + type);

                
                fileName = dp.FileUploadName(path, tf.fileName);

                path = path + fileName;
                //파일이동
                File.Move(sourcePath, path);

                path = imgDomain + "/uploads/" + type + fileName;

                DataRow row = files.NewRow();
                row["fileName"] = fileName;
                row["filePath"] = path;

                files.Rows.InsertAt(row, 0);
            
                try
                {
                    using (var biz = new Api_Biz())
                    {
                        type = "transfer";
                        result = biz.UploadImage(type, tf.tClubIdx, files);
                    }
                }
                catch (Exception e)
                {
                    dic["success"] = false;
                    dic["message"] = "처리중 오류(image)";
                    return dic;
                }
                if (result == -99 || result == -1 || result == -2)
                {
                    dic["success"] = false;
                    dic["message"] = "처리중 오류(image)";
                    return dic;
                }
            }
            else
            {
                path = "";
            }
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.ReqTransfer(tf.memberIdx, tf.befRegionCode, tf.befAreaCode, tf.clubIdx, 
                                             tf.aftRegionCode, tf.aftAreaCode, tf.tClubIdx, path, tf. editReason, reqMember);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if(result == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상처리 되었습니다";
                return dic;

            }
            else
            {
                dic["success"] = false;
                dic["message"] = "database error";
            }
            return dic;
        }

        /// <remarks>
        /// 이적 승인/거절/취소
        /// 
        ///     {
        ///         "transIdx" : 1,   
        ///         "isOk" : 1 (0 : 대기, 1:승인, -1 :거절 , 2 : 보류)
        ///         "editReason" : "그냥마음에 안들어서"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Transfer")]
        public object Put([FromBody] Transfer t)
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

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.EditTransfer(t.transIdx, memberIdx, t.isOk, t.editReason);
                }
            }
            catch (Exception)
            {

                dic["success"] = false;
                dic["message"] = "처리중 오류(image)";
                return dic;
            }

            if (result == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상처리 되었습니다";
                return dic;

            }
            else 
            { 
                dic["success"] = false;
                dic["message"] = "database error";
            }
            return dic;

        }
    }
}

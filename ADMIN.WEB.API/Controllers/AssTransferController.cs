using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AssTransferController : ApiController
    {
        public static string jToken = string.Empty;
        /// <remarks>
        /// 협회관리 이적리스트
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssTransfer")]
        public object Get(string search)
        {
            string server = ConfigurationManager.AppSettings.Get("server");
            Dictionary<string, object> check = new Dictionary<string, object>();

            //개발서버 토큰 필수제외
            if (server != "dev")
            {
                if (Request.Headers.Authorization == null)
                {
                    check["success"] = false;
                    check["message"] = "토큰이 없습니다";
                    return check;
                }

            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            DataSet ds = null;

            JObject root = JObject.Parse(search);
            int id = Convert.ToInt32(root["assIdx"].ToString());
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAssTransferList(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }

            DataTable dt1 = ds.Tables[0];
            DataTable wait = ds.Tables[1];
            DataTable refuse = ds.Tables[2];
            DataTable accept = ds.Tables[3];
            DataTable cancel = ds.Tables[4];

            if (dt1.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["data"] = new DataTable();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt1;
            dic["wait"] = wait.Rows[0]["waitCnt"];
            dic["refuse"] = refuse.Rows[0]["refCnt"];
            dic["accept"] = accept.Rows[0]["accCnt"];
            dic["cancel"] = cancel.Rows[0]["cnlCnt"];
            return dic;
        }


        /// <remarks>
        /// 협회관리 이적 취소
        /// 
        ///     {
        ///         "transIdx" : 2,
        ///         "cancelReason" : "test",
        ///         "assIdx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssTransfer")]
        public object Post([FromBody] AssTransfer at)
        {
            string server = ConfigurationManager.AppSettings.Get("server");
            Dictionary<string, object> check = new Dictionary<string, object>();

            //개발서버 토큰 필수제외
            if (server != "dev")
            {
                if (Request.Headers.Authorization == null)
                {
                    check["success"] = false;
                    check["message"] = "토큰이 없습니다";
                    return check;
                }

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
                    result = biz.CancelTransfer(at.transIdx, at.cancelReason, memberIdx, at.assIdx);
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
                dic["message"] = "정상처리 되었습니다";
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "db error";
            }
            return dic;
        }


        /// <remarks>
        /// 협회관리 이적 상세보기
        /// 
        ///     {
        ///         "transIdx" : 2
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssTransfer/{id}")]
        public object Get(int id)
        {
            string server = ConfigurationManager.AppSettings.Get("server");
            Dictionary<string, object> check = new Dictionary<string, object>();

            //개발서버 토큰 필수제외
            if (server != "dev")
            {
                if (Request.Headers.Authorization == null)
                {
                    check["success"] = false;
                    check["message"] = "토큰이 없습니다";
                    return check;
                }

            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            DataTable dt = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetAssTransferDetail(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }

            dic["success"] = true;
            dic["data"] = dt;
            return dic;
        }
    }
}

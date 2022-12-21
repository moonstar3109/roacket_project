using ADMIN.API.DA;
using ADMIN.WEB.API.Common;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AssAdminPositionController : ApiController
    {
        private string jToken;

        /// <remarks>
        /// 협회별 직책 삭제
        /// 
        ///     {
        ///        "assIdx" : 1,
        ///         "poCode" : 1, 
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminPosition")]
        public object Delete([FromBody] AssAdminPosition ap)
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
                    result = biz.DelAssPosition(ap.assIdx, ap.poCode);
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
            else if (result == -80)
            {
                dic["success"] = false;
                dic["message"] = "사용중인 직위";
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "처리중 오류";
            }

            return dic;
        }


        /// <remarks>
        /// 협회별 직책 수정
        /// 
        ///     {
        ///        "assIdx" : 1,
        ///         "poCode" : 1, 
        ///         "position" : "무법감투꾼",
        ///         "assManage" : 1,
        ///         "noticeManage" : 1,
        ///         "clubManage" : 1,
        ///         "memberManage" : 1,
        ///         "transferManage" : 0
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminPosition")]
        public object Patch([FromBody] AssAdminPosition ap)
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
                    result = biz.EditAssPosition(ap.assIdx, ap.poCode, ap.position, ap.assManage, ap.noticeManage, ap.clubManage, ap.memberManage, ap.transferManage);
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
        /// 협회별 직책 추가
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "positions": [
        ///            {
        ///             "position" : "감투감투"    
        ///            }
        ///         ]
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminPosition")]
        public object Post([FromBody] AssAdminPosition ap)
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

          

            DataTable positions = new DataTable();
            positions.Columns.Add("position", typeof(string));

            for (int i = 0; i < ap.positions.Length; i++)
            {
                DataRow row = positions.NewRow();
                row["position"] = ap.positions[i].position;

                positions.Rows.InsertAt(row, i);
            }

            int result = 0;

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.SetAssPosition(ap.assIdx, positions);
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
        /// 협회별 직책 리스트
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminPosition/{id}")]
        public object Get(int id)
        {
            DataTable dt = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetAssPosition(id);
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
                dic["data"] = new DataTable();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt;
            return dic;
        }
    }
}

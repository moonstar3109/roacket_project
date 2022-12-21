using ADMIN.API.DA;
using ADMIN.WEB.API.Common;
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
    public class AssAdminMemberController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 관리자등록
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "memberIdx" : 1
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminMember")]
        public object Post([FromBody] AssAdminMember a)
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
                    result = biz.SetAssAdmin(a.assIdx, a.memberIdx, memberIdx);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if(result == -80)
            {
                dic["success"] = false;
                dic["message"] = "타 협회 가입된 회원입니다";
                return dic;
            }else if(result == -99)
            {
                dic["success"] = false;
                dic["message"] = "db error";
                return dic;
            }
            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
        }

        /// <remarks>
        /// 관리자 해제
        /// 
        ///     {
        ///        "assIdx" : 1,
        ///        "delList" : [
        ///             {
        ///               "memberIdx" : 2
        ///              },
        ///         ]
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminMember")]
        public object Delete([FromBody] AssAdminMember ass)
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
            int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());
            
            Dictionary<string, object> dic = new Dictionary<string, object>();


            DataTable delList = new DataTable();
            delList.Columns.Add("memberIdx", typeof(int));
            
            for(int i = 0; i < ass.delList.Length; i++)
            {
                DataRow row = delList.NewRow();
                row["memberIdx"] = ass.delList[i].memberIdx;

                delList.Rows.InsertAt(row, i);
            }


            int result = 0; 
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.DeleteAssMember(ass.assIdx, delList, editMember);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if (result == -99 || result != 0)
            {
                dic["success"] = false;
                dic["message"] = "db error";
                return dic;
            }
            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
        }

        /// <remarks>
        /// 관리자 리스트
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "option" : 1, [0 : 전체, 1 : 이름, 2 : 핸드폰]
        ///         "search" : ""
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminMember")]
        public object Get(string filter)
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

            JObject root = JObject.Parse(filter);

            int assIdx = Convert.ToInt32(root["assIdx"].ToString());
            string search = root["search"].ToString() == "" ? string.Empty : root["search"].ToString();
            int option = Convert.ToInt32(root["option"].ToString());
          

            DataSet ds = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAdminMemberList(assIdx, option, search);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable dt1 = ds.Tables[0];

            if(dt1.Rows.Count == 0)
            {
                dic["success"] = false;
                dic["data"] = new DataTable();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt1;
            return dic;
        }

        /// <remarks>
        /// 관리정보 상세조회
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "memberIdx" : 1
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminMember/{id}")]
        public object Get(int id, int memberIdx)
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



            int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataTable dt = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetdAssAdminMemberDetail(id, memberIdx);
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

        /// <remarks>
        /// 관리자 수정
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "memberIdx" : 1,
        ///         "poCode" : 2
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminMember")]
        public object Patch([FromBody] AssAdminMember am)
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

            int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0;
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.EditAssMember(am.assIdx, am.memberIdx, am.poCode, editMember);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if (result == -99 || result != 0)
            {
                dic["success"] = false;
                dic["message"] = "db error";
                return dic;
            }
            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
        }
    }
}

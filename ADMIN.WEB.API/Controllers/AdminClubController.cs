using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;
using Statnco.FW.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AdminClubController : ApiController
    {
        public static string imgDomain = ConfigurationManager.AppSettings.Get("imgDomain").ToString();
        public static string jToken = string.Empty;
        /// <remarks>
        /// 클럽 목록
        /// 
        ///     {
        ///         "region" : 1, 
        ///         "area" : 1,
        ///         "option" : 0, [0:"전체" ,  1 : "클럽명", 2 : "회장명"]
        ///         "search" : "",
        ///         "page" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminClub")]
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
            JObject root = JObject.Parse(filter);
            int region = Convert.ToInt32(root["region"].ToString());
            int area = Convert.ToInt32(root["area"].ToString());
            int option = Convert.ToInt32(root["option"]);
            string search = root["search"].ToString();
            int page = Convert.ToInt32(root["page"].ToString());
            int start = ((page - 1) * 30) + 1;
            int end = page * 30;

            DataSet ds = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetStatncoAdminClubList(region, area, option, search, start, end);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable dt = ds.Tables[0];
            DataTable searchedCnt = ds.Tables[1];
            DataTable clubCnt = ds.Tables[2];
            DataTable delClubCnt = ds.Tables[3];
            DataTable newClubCnt = ds.Tables[4];

            if (dt.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["message"] = "no data";
                return dic;
            }
            dic["success"] = true;
            dic["data"] = dt;
            dic["searchedCnt"] = searchedCnt.Rows[0]["searchedCnt"];
            dic["clubCnt"] = clubCnt.Rows[0]["clubCnt"];
            dic["delClubCnt"] = delClubCnt.Rows[0]["delClubCnt"];
            dic["newClubCnt"] = newClubCnt.Rows[0]["newClubCnt"];
            return dic;
        }

        /// <remarks>
        /// 클럽상세정보
        /// 
        ///     {
        ///        "clubIdx" : 1
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AdminClub/{id}")]
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
            DataSet ds = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAdminClubInfo(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable clubInfo = ds.Tables[0];
            DataTable clubImages = ds.Tables[1];
            DataTable clubMember = ds.Tables[2];
            DataTable clubMemo = ds.Tables[3];
            DataTable canClose = ds.Tables[4];

            dic["success"] = true;
            dic["canClose"] = canClose.Rows[0]["canClose"];
            dic["clubInfo"] = clubInfo.Rows[0].DataRowToJson();
            dic["clubImages"] = clubImages.Rows.Count == 0 ? new DataTable() : clubImages;
            dic["clubMember"] = clubMember;
            dic["clubMemo"] = clubMemo.Rows.Count == 0 ? new DataTable() : clubMemo;

            return dic;

        }

        /// <remarks>
        /// 클럽 메모(롸켓관리자사용)
        /// 
        ///     {
        ///        "clubIdx" : 1,
        ///        "memo" : ""
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AdminClub")]
        public object Post([FromBody] AdminClub ac)
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
                    result = biz.RegistClubMemo(ac.clubIdx, ac.memo,editMember);
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
        /// 클럽 폐쇄
        /// 
        ///     {
        ///        "clubIdx" : 1
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AdminClub")]
        public object Delete([FromBody] AdminClub ac)
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
                    result = biz.DeleteClub(ac.clubIdx, editMember);
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

    }
}

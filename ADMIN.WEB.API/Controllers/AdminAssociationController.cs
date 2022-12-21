using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;
using Statnco.FW.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AdminAssociationController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 협회 목록
        /// 
        ///     {
        ///         "region" : 1, 
        ///         "area" : 1,
        ///         "option" : 0, [0:"전체" ,  1 : "회장명", 2 : "협회명"]
        ///         "search" : "",
        ///         "page" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminAssociation")]
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
                    ds = biz.GetStatncoAdminAssociationList(region, area, option, search, start, end);
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
            DataTable assCnt = ds.Tables[2];
            DataTable regionCnt = ds.Tables[3];
            DataTable areaCnt = ds.Tables[4];

            if (dt.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["message"] = "no data";
                return dic;
            }
            dic["success"] = true;
            dic["data"] = dt;
            dic["searchedCnt"] = searchedCnt.Rows[0]["searchedCnt"];
            dic["assCnt"] = assCnt.Rows[0]["assCnt"];
            dic["regionCnt"] = regionCnt.Rows[0]["regionCnt"];
            dic["areaCnt"] = areaCnt.Rows[0]["areaCnt"];
            return dic;
        }


        /// <remarks>
        /// 협회 상세조회
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminAssociation/{id}")]
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

            DataSet ds = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetStatncoAdminAssociationDetail(id);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable dt = ds.Tables[0];
            DataTable memberList = ds.Tables[1];
            DataTable memo = ds.Tables[2];

            dic["sucess"] = true;
            dic["assInfo"] = dt.Rows[0].DataRowToJson();
            dic["memberList"] = memberList;
            dic["memo"] = memo;

            return dic;


        }

        /// <remarks>
        /// 협회 등록
        /// 
        ///     {
        ///         "region" : 1, 
        ///         "area" : 1,
        ///         "assName" : "",
        ///         "memberIdx" : 5,
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminAssociation")]
        public object Post([FromBody] AdminAssociation aa)
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
                    result = biz.CreateAssociation(aa.region, aa.area, aa.assName, aa.assType, aa.memberIdx, editMember);
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
        /// 협회 메모 등록
        /// 
        ///     {   
        ///         "assIdx" : 3,
        ///         "memo" : ""
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminAssociation")]
        public object Patch([FromBody] AdminAssociation aa)
        {
            //string server = ConfigurationManager.AppSettings.Get("server");
            //Dictionary<string, object> check = new Dictionary<string, object>();

            ////개발서버 토큰 필수제외
            //if (server != "dev")
            //{
            //    if (Request.Headers.Authorization == null)
            //    {
            //        check["success"] = false;
            //        check["message"] = "토큰이 없습니다";
            //        return check;
            //    }

            //}

            ////token 정보 조회
            //jToken = Request.Headers.Authorization.ToString();
            //int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int result = 0;

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.RegistAssMemo(aa.assIdx, aa.memo, 1);
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

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
    public class StadiumRequestController : ApiController
    {

        public static string jToken = string.Empty;

        /// <remarks>
        /// 체육관등록 요청 목록
        /// 
        ///     {
        ///         "option" : 0, [0:"전체" ,  1 : "체육관명", 2 : "요청자"]
        ///         "search" : "",
        ///         "page" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("StadiumRequest")]
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
                    ds = biz.GetStatncoAdminStadiumRequestList(option, search, start, end);

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
            DataTable finishCnt = ds.Tables[2];
            DataTable waitCnt = ds.Tables[3];

            dic["success"] = true;
            dic["data"] = dt;
            dic["searchedCnt"] = searchedCnt.Rows[0]["searchedCnt"];
            dic["finishCnt"] = finishCnt.Rows[0]["finishCnt"];
            dic["waitCnt"] = waitCnt.Rows[0]["waitCnt"];
            return dic;
        }



        /// <remarks>
        /// 체육관 요청 상세정보
        /// 
        ///     {
        ///         "requestIdx" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("StadiumRequest/{id}")]
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

            DataTable dt = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetStatncoAdminStadiumRequestDetail(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }


            dic["success"] = true;
            dic["data"] = dt.Rows[0].DataRowToJson();
            return dic;
        }

        /// <remarks>
        /// 체육관 요청 처리
        /// 
        ///     {
        ///         "requestIdx" : 1,
        ///         "regionCode" : 1, 
        ///         "areaCode" : 2,
        ///         "type" : 1,
        ///         "status" : 2,
        ///         "stadiumName" : "test1234",
        ///         "stadiumAddr" : "서울시 은평구",
        ///         "stadiumAddr2" : "aaaaaaa",
        ///         "phone" : "01015123452",
        ///         "searchList" : "12g, 5u3,  glk34",
        ///         "isUse" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("StadiumRequest")]
        public object Post([FromBody] StadiumRequest sr)
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
            //체육관 상세정보 변경
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.SetStatncoAdminStadiumRequestEdit(sr.requestIdx, sr.regionCode, sr.areaCode, sr.type, sr.status,
                                                                   sr.stadiumName, sr.stadiumAddr, sr.stadiumAddr2, sr.phone, sr.searchList, sr.isUse, editMember);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if (result != 0)
            {
                dic["success"] = false;
                dic["message"] = "상세정보 변경중 오류";
                return dic;
            }

            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
        }
    }
}

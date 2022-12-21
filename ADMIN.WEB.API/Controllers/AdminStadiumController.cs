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
    public class AdminStadiumController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 체육관 목록
        /// 
        ///     {
        ///         "option" : 0, [0:"전체" ,  1 : "시도", 2 : "시군구", 3: "체육관명" , 4: "주소", 5:"미등록"]
        ///         "search" : "",
        ///         "page" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminStadium")]
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
                    ds = biz.GetStatncoAdminStadiumList(option, search, start, end);
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
            DataTable totalCnt = ds.Tables[2];
            DataTable regCnt = ds.Tables[3];
            DataTable openCnt = ds.Tables[4];

            dic["success"] = true;
            dic["data"] = dt;
            dic["searchedCnt"] = searchedCnt.Rows[0]["searchedCnt"];
            dic["totalCnt"] = totalCnt.Rows[0]["totalCnt"];
            dic["regCnt"] = regCnt.Rows[0]["regCnt"];
            dic["openCnt"] = openCnt.Rows[0]["openCnt"];
            return dic;
        }


        /// <remarks>
        /// 체육관 상세정보
        /// 
        ///     {
        ///         "stadiumIdx" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminStadium/{id}")]
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
                    ds = biz.GetStatncoAdminStadiumDetail(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable dt = ds.Tables[0];
            DataTable memo = ds.Tables[1];
            DataTable useStadium = ds.Tables[2];
            DataTable reqMember = ds.Tables[3];

            dic["success"] = true;
            dic["reqMember"] = reqMember.Rows[0].DataRowToJson();
            dic["stadiumInfo"] = dt.Rows[0].DataRowToJson();
            dic["memo"] = memo;
            dic["useStadium"] = useStadium;

            return dic;
        }



        /// <remarks>
        /// 체육관 수정
        /// 
        ///     {   
        ///         "stadiumIdx" : 1,
        ///         "regionCode" : 2,
        ///         "areaCode" : 1,
        ///         "type" : 2,
        ///         "status" : 1,
        ///         "stadiumName" : "test",
        ///         "stadiumAddr" : "서울시 은평구 구산동",
        ///         "stadiumAddr2" : "거북맨션 301호",
        ///         "memo" : "",
        ///         "phone" : "01012345676",
        ///         "searchList": "h1, h2, h3, h4",
        ///         "isUse" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminStadium")]
        public object Patch([FromBody] Stadium s)
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

            //메모가 들어오는 경우
            if (s.memo != string.Empty)
            {
                try
                {
                    using (var biz = new Api_Biz())
                    {
                        result = biz.EditStadiumMemo(s.stadiumIdx, s.memo, editMember);
                    }
                }
                catch (Exception e)
                {
                    dic["success"] = false;
                    dic["message"] = e.ToString();
                    return dic;
                }
                
                if(result != 0)
                {
                    dic["success"] = false;
                    dic["message"] = "메모 작성중 오류";
                    return dic;
                }
            }


            //체육관 상세정보 변경
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.EditStadiumDetail(s.stadiumIdx, s.regionCode, s.areaCode, s.type, s.status,
                                                   s.stadiumName, s.stadiumAddr,s.stadiumAddr2, s.phone, s.searchList, s.isUse, editMember);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if(result != 0)
            {
                dic["success"] = false;
                dic["message"] = "상세정보 변경중 오류";
                return dic;
            }

            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
        }


        /// <remarks>
        /// 체육관 등록
        /// 
        ///     {   
        ///         "regionCode" : 2,
        ///         "areaCode" : 1,
        ///         "type" : 2,
        ///         "status" : 1,
        ///         "stadiumName" : "test",
        ///         "stadiumAddr" : "서울시 은평구 구산동",
        ///         "stadiumAddr2" : "거북맨션 301호",
        ///         "memo" : "",
        ///         "phone" : "01012345676",
        ///         "searchList": "h1, h2, h3, h4",
        ///         "isUse" : 1
        ///         
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminStadium")]
        public object Post([FromBody] Stadium s)
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
                    result = biz.CreateStadium( s.regionCode, s.areaCode, s.type, s.status,
                                                s.stadiumName, s.stadiumAddr, s.stadiumAddr2, s.phone, s.searchList, s.isUse, editMember);
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

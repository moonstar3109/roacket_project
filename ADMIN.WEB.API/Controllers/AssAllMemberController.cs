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
    public class AssAllMemberController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 협회의 회원 목록
        /// 
        ///     {   
        ///           "assIdx" : 1,
        ///           "region" : 1,
        ///           "area" : 2,
        ///           "option" : 0 ,  [ 0 : 전체, 1 : 클럽명 , 2:이름검색, 3:번호],
        ///           "option2" : 1, [0 : 전체, 1 : 급수미인증회원만 검색]
        ///           "search" : "",
        ///           "page" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssAllMember")]
        public object Get(string filter)
        {
            JObject root = JObject.Parse(filter);

            int assIdx = Convert.ToInt32(root["assIdx"].ToString());
            int region = Convert.ToInt32(root["region"].ToString());
            int area = Convert.ToInt32(root["area"].ToString());
            int option = Convert.ToInt32(root["option"].ToString());
            int option2 = Convert.ToInt32(root["option2"].ToString());
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
                    ds = biz.GetAssAllMember(assIdx,region, area, option, option2, search, start, end);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            DataTable dt3 = ds.Tables[2];

            if (dt1.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["data"] = new DataTable();
                dic["searchedCnt"] = dt2.Rows[0]["searchedCnt"];
                dic["totalCnt"] = dt3.Rows[0]["totalCnt"];
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt1;
            dic["searchedCnt"] = dt2.Rows[0]["searchedCnt"];
            dic["totalCnt"] = dt3.Rows[0]["totalCnt"];

            return dic;
        }

        /// <remarks>
        /// 협회의 회원 상세조회
        /// 
        ///     {   
        ///          "memberIdx" : 13
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssAllMember/{clubIdx}/{assIdx}/{memberIdx}")]
        public object Get(int clubIdx, int assIdx, int memberIdx)
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
                    ds = biz.GetAssAllMemberDetail(clubIdx,assIdx, memberIdx);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }

            DataTable memberInfo = ds.Tables[0];
            DataTable regionClass = ds.Tables[1];
            DataTable areaClass = ds.Tables[2];
            DataTable grade = ds.Tables[3];
            DataTable club = ds.Tables[4];
            DataTable transfer = ds.Tables[5];
            DataTable expulsion = ds.Tables[6];
            DataTable regionClassList = ds.Tables[7];
            DataTable areaClassList = ds.Tables[8];

            dic["success"] = true;
            dic["memberInfo"] = memberInfo;
            dic["regionClass"] = regionClass.Rows.Count > 0 ? regionClass.Rows[0].DataRowToJson() : new { };
            dic["areaClass"] = areaClass.Rows.Count > 0 ? areaClass.Rows[0].DataRowToJson() : new { };
            dic["grade"] = grade.Rows.Count > 0 ? grade : new DataTable();
            dic["club"] = club.Rows.Count > 0 ? club : new DataTable();
            dic["transfer"] = transfer.Rows.Count > 0 ? transfer : new DataTable();
            dic["expulsion"] = expulsion.Rows.Count > 0 ? expulsion : new DataTable();
            dic["regionClassList"] = regionClassList.Rows.Count > 0 ? regionClassList : new DataTable();
            dic["areaClassList"] = areaClassList.Rows.Count > 0 ? areaClassList : new DataTable();

            return dic;
        }



        /// <remarks>
        /// 협회의 회원 수정
        /// 
        ///     {   
        ///         "memberIdx" : 3,
        ///         "clubIdx" : 6,
        ///         "birth" : "19920925",
        ///         "phone" : "01051216647",
        ///         "gender" : "M",
        ///         "assIdx" : "1",
        ///         "regionClassCode": 1,
        ///         "areaClassCode": 1,
        ///         "reason": "시합에서 떨어져서 떨어짐",
        ///         "dressCode" : 12,
        ///         "shoesCode" : 11,
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssAllMember")]
        public object Patch([FromBody] AssAllMember aam)
        {
            string server = ConfigurationManager.AppSettings.Get("server");
            Dictionary<string, object> check = new Dictionary<string, object>();

            int editMember = 0;

            //개발서버 토큰 필수제외
            if (server != "dev")
            {
                if (Request.Headers.Authorization == null)
                {
                    check["success"] = false;
                    check["message"] = "토큰이 없습니다";
                    return check;
                }

                //token 정보 조회
                jToken = Request.Headers.Authorization.ToString();

                editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());
            }
            else
            {
                editMember = 2;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0;
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.EditAssAllMemberInfo(aam.memberIdx, aam.clubIdx,aam.birth, aam.phone,
                                                     aam.gender, 
                                                     aam.assIdx, aam.regionClassCode, aam.areaClassCode, aam.reason,
                                                     aam.dressCode, aam.shoesCode, editMember);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if(result  == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상 처리 되었습니다";
                return dic;
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "db error";
                return dic;
            }
        }
    }
}

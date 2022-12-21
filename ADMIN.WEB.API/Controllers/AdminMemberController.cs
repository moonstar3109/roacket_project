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
    public class AdminMemberController : ApiController
    {

        public static string jToken = string.Empty;


        /// <remarks>
        /// 회원 목록
        /// 
        ///     {
        ///         "block" : 0, [0 : 전체 , 1 : 차단]
        ///         "option" : 0, [0:"전체" ,  1 : "이름", 2 : "핸드폰"]
        ///         "search" : "",
        ///         "page" : 1
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AdminMember")]
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
            int block = Convert.ToInt32(root["block"].ToString());
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
                    ds = biz.GetStatncoAdminMemberList(block, option, search, start, end);
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
            DataTable memberCnt = ds.Tables[2];
            DataTable delCnt = ds.Tables[3];
            DataTable signCnt = ds.Tables[4];
            DataTable blockCnt = ds.Tables[5];

            if (dt.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["message"] = "no data";
                return dic;
            }
            dic["success"] = true;
            dic["data"] = dt;
            dic["searchedCnt"] = searchedCnt.Rows[0]["searchedCnt"];
            dic["memberCnt"] = memberCnt.Rows[0]["memberCnt"];
            dic["delCnt"] = delCnt.Rows[0]["delCnt"];
            dic["signCnt"] = signCnt.Rows[0]["signCnt"];
            dic["blockCnt"] = blockCnt.Rows[0]["blockCnt"];
            return dic;
        }


        /// <remarks>
        /// 회원 상세보기
        /// 
        ///     {
        ///        "memberIdx" : 2
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AdminMember/{regAssIdx}/{areAssIdx}/{id}")]
        public object Get(int regAssIdx, int areAssIdx, int id)
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
                    ds = biz.GetStatncoAdminMemberDetail(regAssIdx, areAssIdx, id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable memberInfo = ds.Tables[0];
            DataTable memberAddInfo = ds.Tables[1];
            DataTable clubList = ds.Tables[2];
            DataTable transfer = ds.Tables[3];
            DataTable regionClass = ds.Tables[4];
            DataTable areaClass = ds.Tables[5];
            DataTable classLog = ds.Tables[6];
            DataTable block = ds.Tables[7];
            DataTable blockLog = ds.Tables[8];

            if (memberInfo.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["message"] = "no data";
                return dic;
            }
            dic["success"] = true;
            dic["memberInfo"] = memberInfo.Rows[0].DataRowToJson();
            dic["memberAddInfo"] = memberAddInfo.Rows[0].DataRowToJson();
            dic["clubList"] = clubList.Rows.Count == 0 ? new DataTable() : clubList;
            dic["transfer"] = transfer.Rows.Count == 0 ? new DataTable() : transfer;
            dic["regionClass"] = regionClass.Rows.Count == 0 ? new { } : regionClass.Rows[0].DataRowToJson();
            dic["areaClass"] = areaClass.Rows.Count == 0 ? new { } : areaClass.Rows[0].DataRowToJson();
            dic["classLog"] = classLog.Rows.Count == 0 ? new DataTable() : classLog ;
            dic["block"] = block.Rows.Count == 0 ? new DataTable() : block;
            dic["blockLog"] = blockLog.Rows.Count == 0 ? new DataTable() : blockLog;
            return dic;
        }


        /// <remarks>
        /// 회원 탈퇴
        /// 
        ///     {
        ///        "memberIdx" : 2,
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AdminMember")]
        public object Delete([FromBody] AdminMember am)
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
                    result = biz.DelMember(am.memberIdx, editMember);
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
                dic["message"] = "db error";
                return dic;
            }

            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
        }


        /// <remarks>
        /// 회원 개별 추가
        /// 
        ///     {
        ///        "assIdx" : 1,
        ///        "regionCode" : 2,
        ///        "memberList" : [
        ///             {
        ///                 "areaCode" : 1,
        ///                 "clubIdx": 4,
        ///                 "memberName" : "김팔도",
        ///                 "gender" : "M",
        ///                 "birth" : "19650423",
        ///                 "phone" : "01025537825",
        ///                 "regionClassCode" : 1,
        ///                 "areaClassCode" : 2
        ///             }
        ///         ]
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AdminMember")]
        public object Post ([FromBody] AdminMember am)
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
           
            DataTable memberList = new DataTable();
            memberList.Columns.Add("areaCode", typeof(int));
            memberList.Columns.Add("clubIdx", typeof(int));
            memberList.Columns.Add("memberName", typeof(string));
            memberList.Columns.Add("gender", typeof(string));
            memberList.Columns.Add("birth", typeof(string));
            memberList.Columns.Add("phone", typeof(string));
            memberList.Columns.Add("regionClassCode", typeof(int));
            memberList.Columns.Add("areaClassCode", typeof(int));

            for(int i = 0; i < am.memberList.Length; i++)
            {
                DataRow formatRow = memberList.NewRow();

                formatRow["areaCode"] = am.memberList[i].areaCode;
                formatRow["clubIdx"] = am.memberList[i].clubIdx;
                formatRow["memberName"] = am.memberList[i].memberName;
                formatRow["gender"] = am.memberList[i].gender;
                formatRow["birth"] = am.memberList[i].birth;
                formatRow["phone"] = am.memberList[i].phone;
                formatRow["regionClassCode"] = am.memberList[i].regionClassCode;
                formatRow["areaClassCode"] = am.memberList[i].areaClassCode;

                memberList.Rows.Add(formatRow);
            }

            int result = 0;

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.AddMemberSeperate(am.assIdx, am.regionCode, memberList ,editMember);
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
                dic["message"] = "db error";
                return dic;
            }
            else
            {
                dic["success"] = true;
                dic["message"] = "정상처리 되었습니다";
                return dic;

            }
        }
    }

}

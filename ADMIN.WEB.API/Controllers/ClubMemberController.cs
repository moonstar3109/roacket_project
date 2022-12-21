using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ClubMemberController : ApiController
    {
        /// <remarks>
        /// 클럽멤버리스트
        /// 
        ///     {
        ///         "clubIdx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClubMembers/{id}")]
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
                using(var biz = new Api_Biz())
                {
                    ds = biz.GetClubMemberList(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }

            if(ds.Tables[0].Rows.Count == 0)
            {
                dic["success"] = true;
                dic["message"] = "";
                return dic;
            }

            dic["success"] = true;
            dic["data"] = ds.Tables[0];
            return dic;
        }


        

        /// <remarks>
        /// 클럽멤버 추가
        /// 
        ///     {
        ///         
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClubMember")]
        public object Post([FromBody] ClubMembers cm)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["data"] = "토큰이 없습니다";
                return check;
            }
            string jToken = Request.Headers.Authorization.ToString();
            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            int rtn = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            DataTable dt = new DataTable();

            dt.Columns.Add("memberIdx",typeof(int));
            dt.Columns.Add("clubIdx", typeof(int));
            dt.Columns.Add("memberId", typeof(string));
            dt.Columns.Add("memberName", typeof(string));
            dt.Columns.Add("gender", typeof(string));
            dt.Columns.Add("birth", typeof(string));
            dt.Columns.Add("gradeCode", typeof(int));
            dt.Columns.Add("phone",typeof(string));
            dt.Columns.Add("regionClassCode",typeof(int));
            dt.Columns.Add("areaClassCode",typeof(int));
            dt.Columns.Add("addr1",typeof(string));
            dt.Columns.Add("addr2",typeof(string));
            dt.Columns.Add("dressCode",typeof(int));
            dt.Columns.Add("shoesCode",typeof(int));

            for(int i = 0; i < cm.members.Length; i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["memberIdx"] = cm.members[i].memberIdx;
                dt.Rows[i]["clubIdx"] = cm.members[i].clubIdx;
                dt.Rows[i]["memberId"] = cm.members[i].memberId;
                dt.Rows[i]["memberName"] = cm.members[i].memberName;
                dt.Rows[i]["gender"] = cm.members[i].gender;
                dt.Rows[i]["birth"] = cm.members[i].birth;
                dt.Rows[i]["gradeCode"] = cm.members[i].gradeCode;
                dt.Rows[i]["phone"] = cm.members[i].phone;
                dt.Rows[i]["regionClassCode"] = cm.members[i].regionClassCode;
                dt.Rows[i]["areaClassCode"] = cm.members[i].areaClassCode;
                dt.Rows[i]["addr1"] = cm.members[i].addr1;
                dt.Rows[i]["addr2"] = cm.members[i].addr2;
                dt.Rows[i]["dressCode"] = cm.members[i].dressCode;
                dt.Rows[i]["shoesCode"] = cm.members[i].shoesCode;
            }

            try
            {
                using (var biz = new Api_Biz())
                {
                    rtn = biz.SetClubMember(dt, memberIdx);
                }
            }
            catch (Exception exp)
            {
                dic["success"] = false;
                dic["data"] = rtn;
                return dic;
            }

            dic["success"] = true;
            dic["data"] = "정상처리 되었습니다";
            return dic;
        }

        /// <remarks>
        /// 클럽멤버수정
        /// 
        ///     {
        ///         "memberIdx" : 1, 
        ///         "clubIdx": 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClubMember")]
        public object Put([FromBody] ClubMembers.EditMember m)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();

            string jToken = Request.Headers.Authorization.ToString();
            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            int result = 0;

            

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.EditMemberInfo(m.memberIdx, m.clubIdx, m.birth, m.phone, m.gender, m.gradeCode, m.regionClassCode, m.areaClassCode,
                                                m.addr1, m.addr2, m.dressCode, m.shoesCode);
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
        /// 클럽멤버상세조회
        /// 
        ///     {
        ///         "id" : 0,
        ///         "clubIdx": 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClubMember")]
        public object Get(int id, int clubIdx)
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
                    ds = biz.GetClubMemberInfo(id, clubIdx);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }

            //DataTable memberInfo = ds.Tables[0];
            //memberInfo.Columns.Add("classEdit", typeof(DataTable));
            //memberInfo.Columns.Add("clubName", typeof(string));
            //memberInfo.Columns.Add("clubTransfer", typeof(DataTable));
            ////memberInfo.Columns.Add("association", typeof(DataTable));
            //memberInfo.Columns.Add("expulsion", typeof(DataTable));


            ////DataTable association = ds.Tables[1].Rows.Count == 0 ? new DataTable() : ds.Tables[1];
            ////memberInfo.Rows[0]["association"] = association;

            //DataTable classEdit = ds.Tables[1].Rows.Count == 0 ? new DataTable() : ds.Tables[2];
            //memberInfo.Rows[0]["classEdit"] = classEdit;

            //memberInfo.Rows[0]["clubName"] = ds.Tables[2].Rows[0]["clubName"].ToString();

            //DataTable clubTransfer = ds.Tables[3].Rows.Count == 0 ? new DataTable() : ds.Tables[4];
            //memberInfo.Rows[0]["clubTransfer"] = clubTransfer;

            //DataTable expulsion = ds.Tables[4].Rows.Count == 0 ? new DataTable() : ds.Tables[5];
            //memberInfo.Rows[0]["expulsion"] = expulsion;


            //dic["success"] = true;
            //dic["data"] = memberInfo;

            //return dic;
            DataTable memberInfo = ds.Tables[0];
            DataTable regionClass = ds.Tables[1];
            DataTable areaClass = ds.Tables[2];
            DataTable grade = ds.Tables[3];
            DataTable club = ds.Tables[4];
            DataTable transfer = ds.Tables[5];
            DataTable expulsion = ds.Tables[6];

            dic["success"] = true;
            dic["memberInfo"] = memberInfo;
            dic["regionClass"] = regionClass;
            dic["areaClass"] = areaClass;
            dic["grade"] = grade;
            dic["club"] = club;
            dic["transfer"] = transfer;
            dic["expulsion"] = expulsion;

            return dic;
        }

    }
}

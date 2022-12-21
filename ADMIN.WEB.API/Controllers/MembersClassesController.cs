using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;

namespace ADMIN.WEB.API.Controllers
{
    public class MembersClassesController : ApiController
    {
        public static string jToken = string.Empty;
        /// <remarks>
        /// 멤버 급수 일괄변경
        /// 
        ///     {
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("MembersClasses")]
        public object Post([FromBody] MembersClasses mc)
        {

            int rtn = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();



            DataTable dtRegion = new DataTable();

            dtRegion.Columns.Add("memberIdx", typeof(int));
            dtRegion.Columns.Add("regionClassCode", typeof(int));

            DataTable dtArea = new DataTable();

            dtArea.Columns.Add("memberIdx", typeof(int));
            dtArea.Columns.Add("areaClassCode", typeof(int));

            foreach (MembersClasses.RegionClass rc in mc.regionClasses)
            {
                DataRow dr = dtRegion.NewRow();
                dr["memberIdx"] = rc.memberIdx;
                dr["regionClassCode"] = rc.regionClassCode;
                dtRegion.Rows.Add(dr);
            }

            foreach (MembersClasses.AreaClass ac in mc.areaClasses)
            {
                DataRow dr = dtArea.NewRow();
                dr["memberIdx"] = ac.memberIdx;
                dr["areaClassCode"] = ac.areaClassCode;
                dtArea.Rows.Add(dr);
            }

            try
            {
                using (var biz = new Api_Biz())
                {
                    rtn = biz.SetClubMembersClasses(dtRegion, dtArea, 1);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }

            dic["success"] = true;
            dic["data"] = rtn;
            return dic;
        }

        /// <remarks>
        /// 멤버 급수 조회
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "region" : 2,
        ///         "area" : 2,
        ///         "option" : 0 , [0 : 전체, 1 : 클럽명 , 2: 이름]
        ///         "search" : ""
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("MembersClasses")]
        public object Get(string filter)
        {
            JObject root = JObject.Parse(filter);
            int assIdx = Convert.ToInt32(root["assIdx"].ToString());
            int region = Convert.ToInt32(root["region"].ToString());
            int area = Convert.ToInt32(root["area"]);
            int option = Convert.ToInt32(root["option"]);
            string search = root["search"].ToString();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            DataSet ds = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetClubMembersClasses(assIdx, region, area, option, search);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }

            DataTable data = ds.Tables[0];
            DataTable total = ds.Tables[1];

            if(data.Rows.Count > 0)
            {
                dic["success"] = true;
                dic["data"] = data;
                dic["totalCnt"] = total.Rows[0]["totalCnt"];
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "data empty";

            }

            return dic;
        }



        /// <remarks>
        /// 멤버 급수 수정
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "assType" : 1,
        ///         "reason" : "사,유",
        ///         "classesList" : [
        ///             {
        ///                 "clubIdx" : 1,
        ///                 "memberIdx" : 256,
        ///                 "classCode" : 20
        ///             }
        ///         ]
        ///        
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("MembersClasses")]
        public object Patch([FromBody] EditMemberClass mc)
        {

            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();
            int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            Dictionary<string, object> dic = new Dictionary<string, object>();
            int rtn = 0;

            DataTable classList = new DataTable();
            classList.Columns.Add("clubIdx", typeof(int));
            classList.Columns.Add("memberIdx", typeof(int));
            classList.Columns.Add("classCode", typeof(int));

            for (int i = 0; i < mc.classesList.Length; i++)
            {
                DataRow formatRow = classList.NewRow();

                formatRow["clubIdx"] = mc.classesList[i].clubIdx;
                formatRow["memberIdx"] = mc.classesList[i].memberIdx;
                formatRow["classCode"] = mc.classesList[i].classCode;

                classList.Rows.Add(formatRow);
            }

            try
            {
                using (var biz = new Api_Biz())
                {
                    rtn = biz.EditClubMembersClasses(mc.assIdx, mc.assType, mc.reason, classList, editMember);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }
            if(rtn == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상처리";
            }
            else 
            {
                dic["success"] = false;
                dic["message"] = "DB error";
            }

            return dic;
        }
    }
}

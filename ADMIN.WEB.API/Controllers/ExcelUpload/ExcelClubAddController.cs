using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers.ExcelUpload
{
    public class ExcelClubAddController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 엑셀 클럽 추가
        /// 
        ///     {   
        ///           "assIdx" : 2,
        ///           "clubList" : [
        ///             {
        ///              "region" : "대구",
        ///              "area" : "동구",
        ///              "clubName" : "너무하고싶어",
        ///              },
        ///              {
        ///              "region" : "부산",
        ///              "area" : "영도구",
        ///              "clubName" : "DDNS배드민턴",
        ///              },
        ///             
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ExcelClubAdd")]
        public object Post([FromBody] ExcelClubAdd eca)
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

            int assIdx = eca.assIdx;

            DataTable clubList = new DataTable();

            clubList.Columns.Add("region", typeof(string));
            clubList.Columns.Add("area", typeof(string));
            clubList.Columns.Add("clubName", typeof(string));

            Dictionary<string, object> dic = new Dictionary<string, object>();

            for (int i = 0; i < eca.clubList.Length; i++)
            {
                DataRow row = clubList.NewRow();
                row["region"] = eca.clubList[i].region;
                row["area"] = eca.clubList[i].area;
                row["clubName"] = eca.clubList[i].clubName;

                clubList.Rows.InsertAt(row, i);
            }

            DataTable dt = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.AddClubToExcel(assIdx, clubList, editMember);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if (dt.Rows.Count > 0)
            {
                dic["success"] = false;
                dic["data"] = dt;
            }
            else
            {
                dic["success"] = true;
                dic["message"] = "정상처리";

            }

            return dic;
        }

    }
}

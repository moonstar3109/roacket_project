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

namespace ADMIN.WEB.API.Controllers.Compare
{
    public class ClassCompareController : ApiController
    {
        public static string jToken = string.Empty;
        /// <remarks>
        /// 급수 비교
        /// 
        ///     {   
        ///           "assType" : 1,
        ///           "checkList" : [
        ///             {
        ///              "clubIdx" : 2,
        ///              "memberIdx" : 13,
        ///              "classCode" : "B"
        ///              },
        ///              {
        ///              "clubIdx" : 2,
        ///              "memberIdx" : 15,
        ///              "classCode" : "B"
        ///              }      
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClassCompare")]
        public object Post([FromBody] ClassCompare cc)
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

            int assType = cc.assType;

            DataTable checkList = new DataTable();
            checkList.Columns.Add("clubIdx", typeof(int));
            checkList.Columns.Add("memberIdx", typeof(int));
            checkList.Columns.Add("classCode", typeof(string));

            Dictionary<string, object> dic = new Dictionary<string, object>();

            for(int i = 0; i < cc.checkList.Length; i++)
            {
                DataRow row = checkList.NewRow();
                row["clubIdx"] = cc.checkList[i].clubIdx;
                row["memberIdx"] = cc.checkList[i].memberIdx;
                row["classCode"] = cc.checkList[i].classCode;

                checkList.Rows.InsertAt(row, i);
            }

            DataTable dt = null;
            try
            {
                using (var biz = new Api_Biz()) 
                {
                    dt = biz.CheckMemberClassCompare(assType, editMember, checkList);

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

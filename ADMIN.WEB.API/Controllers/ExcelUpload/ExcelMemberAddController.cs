using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers.ExcelUpload
{
    public class ExcelMemberAddController : ApiController
    {
        public static string jToken = string.Empty;
        /// <remarks>
        /// 엑셀 회원 추가
        /// 
        ///     {   
        ///           "assType" : 1,
        ///           "memberList" : [
        ///             {
        ///              "clubIdx" : 2,
        ///              "memberName" : "김배드민턴",
        ///              "gender" : "M",
        ///              "birth" : "19920405",
        ///              "phone" : "01023631042",
        ///              "classCode" : "A",
        ///              },
        ///              {
        ///              "clubIdx" : 2,
        ///              "memberName" : "김배도민턴",
        ///              "gender" : "M",
        ///              "birth" : "19920205",
        ///              "phone" : "01023631042",
        ///              "classCode" : "B"
        ///              }
        ///         ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ExcelMemberAdd")]
        public object Post([FromBody] ExcelMemberAdd e)
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

            int assType = e.assType;

            DataTable memberList = new DataTable();
            memberList.Columns.Add("clubIdx", typeof(int));
            memberList.Columns.Add("memberName", typeof(string));
            memberList.Columns.Add("gender", typeof(string));
            memberList.Columns.Add("birth", typeof(string));
            memberList.Columns.Add("phone", typeof(string));
            memberList.Columns.Add("classCode", typeof(string));

            Dictionary<string, object> dic = new Dictionary<string, object>();

            for (int i = 0; i < e.memberList.Length; i++)
            {
                DataRow row = memberList.NewRow();
                row["clubIdx"] = e.memberList[i].clubIdx;
                row["memberName"] = e.memberList[i].memberName;
                row["gender"] = e.memberList[i].gender;
                row["birth"] = e.memberList[i].birth;
                row["phone"] = e.memberList[i].phone;
                row["classCode"] = e.memberList[i].classCode;   

                memberList.Rows.InsertAt(row, i);
            }

            DataTable dt = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.AddMemberToExcel(assType, memberList, editMember);
                }
            }
            catch (Exception ef)
            {

                dic["success"] = false;
                dic["message"] = ef.ToString();
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

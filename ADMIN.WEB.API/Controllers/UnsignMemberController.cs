using ADMIN.API.DA;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class UnsignMemberController : ApiController
    {

        public static string jToken = string.Empty;


        /// <remarks>
        /// 미가입회원등록시 중복확인
        /// 
        ///     {
        ///         "clubIdx" : 1,
        ///         "memberName" : "김아무개",
        ///         "gender" : "M",
        ///         "birth" : "19920925"
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("UnsingMemberDup")]
        public object Get(string filter)
        {
            string server = ConfigurationManager.AppSettings.Get("server");
            Dictionary<string, object> check = new Dictionary<string, object>();

            //개발서버 토큰 필수제외
            //if (server != "dev")
            //{
            //    if (Request.Headers.Authorization == null)
            //    {
            //        check["success"] = false;
            //        check["message"] = "토큰이 없습니다";
            //        return check;
            //    }

            //}
            JObject root = JObject.Parse(filter);
            int clubIdx = Convert.ToInt32(root["clubIdx"].ToString());
            string memberName = root["memberName"].ToString();
            string gender = root["gender"].ToString();
            string birth = root["birth"].ToString();
            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0; 
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.GetDuplicateUnsingMember(clubIdx, memberName,gender,birth);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }


            if(result == 1)
            {
                dic["success"] = false;
                dic["message"] = "동명이인";
            }
            else if (result == 0)
            {
                dic["success"] = true;
                dic["message"] = "사용가능";
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

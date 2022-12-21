using ADMIN.API.DA;
using ADMIN.WEB.API.Common;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class NaverLoginController : ApiController
    {
        public static string jToken = string.Empty;

        [Route("Naver")]
        public object Post([FromBody] Naver nv)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var NL = new NaverLoginApi();
            dic = NL.NaverLogin(nv.CT);

            int status = Convert.ToInt32(dic["status"].ToString());

            Dictionary<string, object> res = new Dictionary<string, object>();

            if (status == 200)
            {
                string text = dic["result"].ToString();

                JObject root = JObject.Parse(text);
                JObject response = (JObject)root["response"];

                string id = response["id"].ToString();
                //string nickname = response["nickname"].ToString();
                //string gender = response["gender"].ToString();
                //string email = response["email"].ToString();
                //string mobile = response["mobile"].ToString().Replace("-", "");
                //string birth = response["birthyear"].ToString() + "-" + response["birthday"].ToString();
                //string name = response["name"].ToString();



                res["status"] = status;
                res["id"] = id;
                //res["nickname"] = nickname;
                //res["gender"] = gender;
                //res["email"] = email;
                //res["mobile"] = mobile;
                //res["birth"] = birth;
                //res["name"] = name;

            }
            else
            {
                res["status"] = status;
                res["message"] = "error";
            }
            return res;

        }
    }
}

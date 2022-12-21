using ADMIN.API.DA;
using Newtonsoft.Json.Linq;
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
    public class AdminBoardController : ApiController
    {
        public static string jToken = string.Empty;

        /// <remarks>
        /// 체육관등록 요청 목록
        /// 
        ///     {
        ///         "boardType" : "free",
        ///         "option" : 0,  [0: 전체, 1 : "제목" , 2 : "작성자"]
        ///         "search" : "",
        ///         "page" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AdminBoard")]
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
            string boardType = root["boardType"].ToString();
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
                    ds = biz.GetStatncoAdminBoardList(boardType, option, search, start, end);

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
            DataTable finishCnt = ds.Tables[2];
            DataTable waitCnt = ds.Tables[3];

            dic["success"] = true;
            dic["data"] = dt;
            dic["searchedCnt"] = searchedCnt.Rows[0]["searchedCnt"];
            dic["finishCnt"] = finishCnt.Rows[0]["finishCnt"];
            dic["waitCnt"] = waitCnt.Rows[0]["waitCnt"];
            return dic;
        }


    }
}

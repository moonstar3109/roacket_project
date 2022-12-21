using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
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
    public class AssClubController : ApiController
    {
        public static string jToken = string.Empty;
        public static string imgDomain = ConfigurationManager.AppSettings.Get("imgDomain");

        /// <remarks>
        /// 협회소속 클럽 상세정보
        /// 
        ///     {   
        ///          "clubIdx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssClub/{id}")]
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
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAssClubInfo(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            DataTable clubInfo = ds.Tables[0];
            DataTable clubImages = ds.Tables[1];
            DataTable clubMember = ds.Tables[2];

           for(int i = 0; i < clubImages.Rows.Count; i++)
            {
                clubImages.Rows[i]["filePath"] = imgDomain + clubImages.Rows[i]["filePath"];
            }

            dic["success"] = true;
            dic["clubInfo"] = clubInfo;
            dic["clubImages"] = clubImages;
            dic["clubMember"] = clubMember;
            return dic;
        }

        /// <remarks>
        /// 협회소속 클럽 목록
        /// 
        ///     {   
        ///           "assIdx" : 1,
        ///           "option" : 1, [0 : 전체, 1:클럽명, 2:클럽회장, 3:체육관명]
        ///           "search" : "",
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssClub")]
        public object Get(string filter) 
        {
            JObject root = JObject.Parse(filter);

            int assIdx = Convert.ToInt32(root["assIdx"].ToString());
            int option = Convert.ToInt32(root["option"].ToString());
            string search = root["search"].ToString();


            DataSet ds = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAssClub(assIdx, option, search);
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

            if (dt1.Rows.Count == 0)
            {
                dic["success"] = true;
                dic["data"] = new DataTable();
                dic["clubCnt"] = dt2.Rows[0]["clubCnt"];
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt1;
            dic["clubCnt"] = dt2.Rows[0]["clubCnt"];
            return dic;
        }



        /// <remarks>
        /// 협회소속 클럽 등록
        /// 
        ///     {   
        ///           "assIdx" : 1,
        ///           "registList" : [
        ///             {"clubIdx" : 2},
        ///             {"clubIdx" : 5}
        ///            ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssClub")]
        public object Post([FromBody] AssClub ac)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false; 
                check["message"] = "토큰이 없습니다";
                return check;
            }
            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();
            int editMember = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());


            Dictionary<string, object> dic = new Dictionary<string, object>();

            int result = 0;

            DataTable registList = new DataTable();
            registList.Columns.Add("clubIdx", typeof(int));

            for (int i = 0; i < ac.registList.Length; i++) 
            {
                DataRow row = registList.NewRow();
                row["clubIdx"] = ac.registList[i].clubIdx;

                registList.Rows.InsertAt(row, i);
            }


            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.SetClub(ac.assIdx, registList, editMember);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if(result == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상처리 되었습니다";
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "db error";
            }

            return dic;
        }



        /// <remarks>
        /// 협회소속 클럽 해제
        /// 
        ///     {   
        ///           "assIdx" : 1,
        ///           "clubIdx" : 2
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AssClub")]
        public object Patch([FromBody] AssClub ac)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
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
                    result = biz.CancelClub(ac.assIdx, ac.clubIdx, editMember);
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
                dic["message"] = "정상처리 되었습니다";
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

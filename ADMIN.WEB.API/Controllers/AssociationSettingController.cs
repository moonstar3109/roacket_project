using ADMIN.API.DA;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
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
    public class AssociationSettingController : ApiController
    {
        
        public static string jToken = string.Empty;
        /// <remarks>
        /// 협회 셋팅 정보
        /// 
        ///     {
        ///       "assIdx" : 1
        ///     }
        ///       
        /// </remarks>
        /// <returns></returns>
        [Route("AssociationSetting/{id}")]
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

            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataTable dt = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetStatncoAdminAssociationDetailSetting(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt.Rows[0].DataRowToJson();

            return dic;
        }


        /// <remarks>
        /// 협회 셋팅 변경
        /// 
        ///     {
        ///        "assIdx" : 1,
        ///        "assName" : "변경용",
        ///        "regionCode" : 2,
        ///        "areaCode" : 1,
        ///        "assType" : "A"
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssociationSetting")]
        public object Patch([FromBody]AssociationSetting acs)
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
                    result = biz.EditStatncoAssocationDetailSetting(acs.assIdx,acs.regionCode, acs.areaCode, acs.assName, acs.assType, editMember);
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

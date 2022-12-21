using ADMIN.API.DA;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class TempMemberController : ApiController
    {
        /// <remarks>
        /// 임시회원 체크(네이버 : N, 카카오 : K,  구글 : G , 애플 : A)
        /// 
        ///     {
        ///         "id" : "GX6bigpK5V_cYaVfEQx4uhvjJbaPlDmewUGhClG9aEk",
        ///         "type" : "N"                                        
        ///     }
        /// </remarks>
        /// <param></param>
        /// <returns></returns>
        [Route("TempMember")]
        public object Post([FromBody] TempMember tm)
        {
            int result = 0;
            DataTable dt = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetMember(tm.id, tm.type);
                }
                if (dt.Rows.Count > 0)
                {
                    dic["success"] = true;
                    dic["joinCheck"] = true;
                    dic["isManager"] = dt.Rows[0]["memberType"].ToString() == "U" ? false : true ;
                    return dic;
                }
                else
                {
                    try
                    {
                        // 임시회원등록
                        using (var biz = new Api_Biz())
                        {
                            result = biz.SetTempMember(tm.id, tm.type);
                        }

                    }
                    catch (Exception e)
                    {

                        dic["success"] = false;
                        dic["message"] = "임시 유저 등록중 에러";
                        dic["Exception"] = e.ToString();
                        return dic;
                    }

                    if (result == 0)
                    {
                        dic["success"] = true;
                        dic["joinCheck"] = false;

                    }

                }

            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["exception"] = e.ToString();
            }

            return dic;
        }
    }
}

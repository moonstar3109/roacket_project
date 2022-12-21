using ADMIN.API.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AdminGradeController : ApiController
    {

        private string jToken;


        /// <remarks>
        /// 협회별 등급 리스트
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///     }
        ///      
        /// </remarks>
        /// <returns></returns>
        [Route("AssAdminGrade/{id}")]
        public object Get(int id)
        {
            DataTable dt = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetAssGrade(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if (dt.Rows.Count == 0)
            {
                dic["success"] = false;
                dic["data"] = new DataTable();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt;
            return dic;
        }
    }
}

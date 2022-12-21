 using ADMIN.API.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class DressController : ApiController
    {
        /// <remarks>
        /// 옷사이즈조회
        /// 
        ///     {
        ///         
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Dress")]
        public object Get()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataTable dt = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetDressList();
                }
            }
            catch (Exception exp)
            {
                dic["success"] = false;
                dic["message"] = exp.ToString();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt;

            return dic;
        }

    }
}

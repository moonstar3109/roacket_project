using ADMIN.API.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ShoesController : ApiController
    {
        /// <remarks>
        /// 신발사이즈조회
        /// 
        ///     {
        ///         
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Shoes")]
        public object Get()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataTable dt = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetShoesList();
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

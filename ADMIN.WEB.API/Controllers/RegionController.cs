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
    public class RegionController : ApiController
    {
        [Route("Region")]
        public object Get()
        {
            DataTable dt = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetRegion();
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt;

            return dic;
        }
    }
}

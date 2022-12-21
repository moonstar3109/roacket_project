 using ADMIN.API.DA;
using Statnco.FW.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers.Modal
{
    public class AdminInfoToModalController : ApiController
    {
        [Route("AdminInfo/{id}")]
        public object Get(int id)
        {
            DataTable dt = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetAdminInfoToModal(id);
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
    }
}

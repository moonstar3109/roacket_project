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

    
    public class AssociationListController : ApiController
    {
        /// <remarks>
        /// 협회 리스트 조회
        /// </remarks>
        /// <returns>
        /// </returns>
        [Route("AssociationList")]
        public object Get(int regionCode)
        {
            DataTable dt = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetAssList(regionCode);
                }
            }
            catch (Exception exp)
            {
                dic["success"] = false;
                dic["data"] = exp.Message;
                return dic;
            }

            dic["success"] = true;
            dic["data"] = dt;
            return dic;
        }
    }
}

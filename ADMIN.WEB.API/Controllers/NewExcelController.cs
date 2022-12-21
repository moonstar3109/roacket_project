using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ADMIN.API.DA;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System.Web;

namespace ADMIN.WEB.API.Controllers
{
    public class NewExcelController : ApiController
    {
        /// <remarks>
        /// 협회 급수비교 양식 다운로드
        /// 
        ///     {
        ///         "type" : "class",
        ///         "assType" : 1,
        ///         "semitype": "club", 
        ///         "idx" : 2
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("NewExcel")]
        public object Get(string filter)
        {
            JObject root = JObject.Parse(filter);
            string type = root["type"].ToString();
            int assType = Convert.ToInt32(root["assType"].ToString());
            string semitype = root["semitype"].ToString();
            int idx = Convert.ToInt32(root["idx"].ToString());
            Dictionary<string, object> dic = new Dictionary<string, object>();

            string baseUrl = "http://excel.roacket.com";


            
            //string linkUrl = type == "class" || type == "member" ? "{0}/{1}?assType={2}&semitype={3}&idx={4}" : "{0}/{1}?semitype={3}&idx={4}";

            string fileName = string.Empty;
            string linkUrl = string.Empty;
            switch (type)
            {
                case "member":
                    fileName = "member.aspx";
                    linkUrl = "{0}/{1}?assType={2}&semitype={3}&idx={4}";
                    break;
                case "club":
                    fileName = "club.aspx";
                    break;
                case "class":
                    fileName = "memberClass.aspx";
                    linkUrl = "{0}/{1}?assType={2}&semitype={3}&idx={4}";
                    break;
            }

            dic["success"] = true;
            dic["message"] = String.Format(
                    linkUrl,
                    baseUrl,
                    fileName,
                    assType,
                    semitype,
                    idx
                );

            return dic;
        }
    }
}

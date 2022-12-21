using ADMIN.API.DA;
using ADMIN.WEB.API.Common;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class AddFileController : ApiController
    {
        public static string imgDomain = ConfigurationManager.AppSettings.Get("imgDomain");

        /// <remarks>
        /// 첨부파일목록
        /// 
        ///     {
        ///         "type" : "notice",
        ///         "idx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AddFile")]
        public object Get(string type, int idx)
        {
            DataTable dt = new DataTable();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var down = new Download();

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetFileList(type, idx);
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
                dic["success"] = true;
                dic["message"] = "data empty";
            }
            else
            {
                DataTable list = new DataTable();
                list.Columns.Add("no", typeof(int));
                list.Columns.Add("fileName", typeof(string));
                list.Columns.Add("filePath", typeof(string));
                list.Columns.Add("byte", typeof(string));
                foreach (DataRow row in dt.Rows)
                {

                    DataRow dataRow = list.NewRow();
                    dataRow["no"] = row["no"];
                    dataRow["fileName"] = row["fileName"];
                    dataRow["filePath"] = HttpContext.Current.Server.MapPath("/" + row["FilePath"] + row["fileName"]);

                    if (File.Exists(dataRow["filePath"].ToString()))
                    {
                        dataRow["byte"] = down.GetFile(dataRow["filePath"].ToString());
                    }
                    else
                    {
                        dic["success"] = false;
                        dic["message"] = "file not exist : " + dataRow["filePath"];

                        return dic;
                    }
                    list.Rows.Add(dataRow);
                }
               
                dic["success"] = true;
                dic["data"] = list;
            }
            return dic;
        }
        
    }
}

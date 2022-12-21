using ADMIN.API.DA;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;
using Statnco.FW.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ClassController : ApiController
    {
 
        /// <remarks>
        /// 급수 수정
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "edits" : [
        ///         {
        ///             "stClass" : "",
        ///             "classCode" : 1,
        ///             "className" : "무어라무어라"
        ///             
        ///         } 
        ///        ]
        ///     }   
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Class")]
        public object Patch([FromBody] Classes c)
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


            DataTable editList = new DataTable();
            editList.Columns.Add("stClass", typeof(string));
            editList.Columns.Add("classCode", typeof(int));
            editList.Columns.Add("className", typeof(string));

            for (int i = 0; i < c.edits.Length; i++)
            {
                DataRow row = editList.NewRow();
                row["stClass"] = c.edits[i].stClass;
                row["classCode"] = c.edits[i].classCode;
                row["className"] = c.edits[i].className;

                editList.Rows.InsertAt(row, i);
            }

            int result = 0;

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.EditRegionClass(c.assIdx, editList);
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
                dic["message"] = "정상처리";
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "처리중 오류";
            }

            return dic;
        }


        /// <remarks>
        /// 급수조회
        /// 
        ///     {
        ///         "type" : "club", [club, ass]
        ///         "idx" : 1
        ///     }   
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Class")]
        public object Get(string filter)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataSet ds = null;

            JObject root = JObject.Parse(filter);
            string type = root["type"].ToString();
            int idx = Convert.ToInt32(root["idx"].ToString());

            try
            {
                using(var biz = new Api_Biz())
                {
                    ds = biz.GetClassList(type, idx);
                }
            }
            catch (Exception exp)
            {
                dic["success"] = false;
                dic["message"] = exp.ToString();
                return dic;
            }
            if(type == "club")
            {
                dic["success"] = true;
                dic["data"] = ds.Tables[0];

            }
            else
            {
                DataTable dt1 = ds.Tables[0];
                //DataTable dt2 = ds.Tables[1];

                if(dt1.Rows.Count == 0)
                {
                    DataRow dt2 = new DataTable().NewRow();

                    dic["success"] = true;
                    dic["data"] = dt2.DataRowToJson();
                    return dic;
                }
                Dictionary<string, object> classList = new Dictionary<string, object>();
                Dictionary<string, object> result = new Dictionary<string, object>();

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataTable value1 = new DataTable();
                    value1.Columns.Add("classCode", typeof(int));
                    value1.Columns.Add("className", typeof(string));

                    string stClass = dt1.Rows[i]["stClass"].ToString().Trim();
                    for(int ii = 0; ii < dt1.Rows.Count; ii++)
                    {
                        string stClass2 = dt1.Rows[ii]["stClass"].ToString().Trim();
                        if(stClass == stClass2)
                        {
                            DataRow row = value1.NewRow();
                            row["classCode"] = Convert.ToInt32(dt1.Rows[ii]["classCode"].ToString());
                            row["className"] = dt1.Rows[ii]["className"].ToString();

                            value1.Rows.InsertAt(row, ii);

                            classList[stClass] = value1;
                        }
                    }

                    result = classList;
                }

                dic["success"] = true;
                dic["data"] = result;

                
            }

            return dic;
           
        }
    }
}

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
    public class AreaClassController : ApiController
    {
        /// <remarks>
        /// 시군구 급수 수정
        /// 
        ///     {
        ///         "assIdx" : 2,
        ///         "areas" : [
        ///             {"area" : 1 },
        ///             {"area" : 2 },
        ///             {"area" : 3 }
        ///         ],
        ///         "edits" : [
        ///            {
        ///             "stClass" : "A",
        ///             "classCode" : 0,
        ///             "className" : "다른감투꾼"
        ///             },
        ///         ]
        ///         
        ///     }   
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AreaClass")]
        public object Patch([FromBody] Classes c)
        {
            //string server = ConfigurationManager.AppSettings.Get("server");
            //Dictionary<string, object> check = new Dictionary<string, object>();

            ////개발서버 토큰 필수제외
            //if (server != "dev")
            //{
            //    if (Request.Headers.Authorization == null)
            //    {
            //        check["success"] = false;
            //        check["message"] = "토큰이 없습니다";
            //        return check;
            //    }

            //}


            Dictionary<string, object> dic = new Dictionary<string, object>();


            DataTable areaList = new DataTable();
            areaList.Columns.Add("areaCode", typeof(int));
            for (int i = 0; i < c.areas.Length; i++)
            {
                DataRow row = areaList.NewRow();
                row["areaCode"] = c.areas[i].area;

                areaList.Rows.InsertAt(row, i);
            }

            DataTable classList = new DataTable();
            classList.Columns.Add("stClass", typeof(string));
            classList.Columns.Add("classCode", typeof(int));
            classList.Columns.Add("className", typeof(string));
            for (int ii = 0; ii < c.edits.Length; ii++)
            {
                DataRow row = classList.NewRow();
                row["stClass"] = c.edits[ii].stClass;
                row["classCode"] = c.edits[ii].classCode;
                row["className"] = c.edits[ii].className;

                classList.Rows.InsertAt(row, ii);

            }

            int result = 0;

            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.SetAreaClass(c.assIdx, areaList, classList);
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
        /// 시군구 급수 조회
        /// 
        ///     {
        ///         "type" : 'club',   ['club', 'ass']
        ///         "idx" : 1
        ///     }   
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("AreaClass")]
        public object Get(string filter)
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

            JObject root = JObject.Parse(filter);
            string type = root["type"].ToString();
            int idx = Convert.ToInt32(root["idx"].ToString());

            DataSet ds = null;
            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetAreaClassList(type, idx);
                }
            }
            catch (Exception exp)
            {
                dic["success"] = false;
                dic["message"] = exp.ToString();
                return dic;
            }

            if(type == "ass")
            {
                //ass
                DataTable dt1 = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];

                if (dt1.Rows.Count == 0)
                {
                    DataRow dt3 = new DataTable().NewRow();

                    dic["success"] = true;
                    dic["data"] = dt3.DataRowToJson();
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
                    for (int ii = 0; ii < dt2.Rows.Count; ii++)
                    {
                        string stClass2 = dt2.Rows[ii]["stClass"].ToString().Trim();
                        if (stClass == stClass2)
                        {
                            DataRow row = value1.NewRow();
                            row["classCode"] = Convert.ToInt32(dt2.Rows[ii]["classCode"].ToString());
                            row["className"] = dt2.Rows[ii]["className"].ToString();

                            value1.Rows.InsertAt(row, ii);

                            classList[stClass] = value1;
                        }
                    }

                    result = classList;
                }

                dic["success"] = true;
                dic["data"] = result;
            }
            else
            {
                //club
                dic["success"] = true;
                dic["data"] = ds.Tables[0];
            }

            return dic;
        }
    }
}

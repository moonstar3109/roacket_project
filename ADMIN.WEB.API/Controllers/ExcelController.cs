using ADMIN.API.DA;
using ADMIN.WEB.API.Common;
using Newtonsoft.Json.Linq;
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
    public class ExcelController : ApiController
    {

        /// <remarks>
        /// 협회 급수비교 양식 다운로드
        /// 
        ///     {
        ///         "type" : "notice",
        ///         "idx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Excel")]
        public object Get(string filter)
        {
            JObject root = JObject.Parse(filter);
            string type = root["type"].ToString();
            int idx = Convert.ToInt32(root["idx"].ToString());

            DataTable dt = null;

            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetClassExcelForm(type, idx);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            if (dt.Rows.Count > 0)
            {
                DataTable format = new DataTable();
                format.Columns.Add("번호", typeof(int));
                format.Columns.Add("시도", typeof(string));
                format.Columns.Add("시군구", typeof(string));
                format.Columns.Add("클럽명", typeof(string));
                format.Columns.Add("클럽번호", typeof(int));
                format.Columns.Add("이름", typeof(string));
                format.Columns.Add("회원번호", typeof(int));
                format.Columns.Add("성별", typeof(string));
                format.Columns.Add("회원등급", typeof(string));
                format.Columns.Add("생년월일", typeof(string));
                format.Columns.Add("연락처", typeof(string));
                format.Columns.Add("시도급수", typeof(string));
                format.Columns.Add("시군구급수", typeof(string));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow formatRow = format.NewRow();
                    
                    formatRow["번호"] = dt.Rows[i]["no"];
                    formatRow["시도"] = dt.Rows[i]["region"];
                    formatRow["시군구"] = dt.Rows[i]["area"];
                    formatRow["클럽명"] = dt.Rows[i]["clubName"];
                    formatRow["클럽번호"] = dt.Rows[i]["clubIdx"];
                    formatRow["이름"] = dt.Rows[i]["memberName"];
                    formatRow["회원번호"] = dt.Rows[i]["memberIdx"];
                    formatRow["성별"] = dt.Rows[i]["gender"];
                    formatRow["회원등급"] = dt.Rows[i]["gradeName"];
                    formatRow["생년월일"] = dt.Rows[i]["birth"];
                    formatRow["연락처"] = dt.Rows[i]["phone"] == null ? string.Empty : dt.Rows[i]["phone"];
                    formatRow["시도급수"] = string.Empty;
                    formatRow["시군구급수"] = string.Empty;

                    format.Rows.Add(formatRow);
                }
                dt = format;
                string attachment = "attachment; filename=formData.xls";
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.AddHeader("content-disposition", attachment);
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                //한글 인코딩 지정
                HttpContext.Current.Response.Charset = "euc-kr";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");

                
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    HttpContext.Current.Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");

                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        HttpContext.Current.Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    HttpContext.Current.Response.Write("\n");
                }


                int a = 0; 
                HttpContext.Current.Response.End();
                //Workbook workbook = new Workbook();
                //Worksheet sheet = workbook.Worksheets[0];

                ////Export datatable to excel
                //sheet.InsertDataTable(format, true, 1, 1, -1, -1);

                ////Save the file
                //string dir = HttpContext.Current.Server.MapPath("/uploads/form/").ToString();
                //string path = dir + "급수비교양식.xlsx";
                //workbook.SaveToFile(path, ExcelVersion.Version2013);

                //var down = new Download();

                dic["success"] = true;
                //dic["data"] = down.GetFile(path);
               
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "no data";
            }
            return dic;
        }
    }
}

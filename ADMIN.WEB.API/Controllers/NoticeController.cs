using ADMIN.API.DA;
using ADMIN.WEB.API.Common;
using ADMIN.WEB.API.Jwt;
using ADMIN.WEB.API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class NoticeController : ApiController
    {
        public static string jToken = string.Empty;
        public string imgDomain = ConfigurationManager.AppSettings.Get("imgDomain");

        /// <remarks>
        /// 공지사항 상세조회
        /// 
        ///     {
        ///         "noticeIdx" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Notice/{id}")]
        public object Get(int id)
        {
            //Dictionary<string, object> check = new Dictionary<string, object>();

            //if (Request.Headers.Authorization == null)
            //{
            //    check["success"] = false;
            //    check["message"] = "토큰이 없습니다";
            //    return check;
            //}
            ////token 정보 조회
            //jToken = Request.Headers.Authorization.ToString();


            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataSet ds = null;

            DataTable notice = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetNoticeDetail(id);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            notice = ds.Tables[0];
            notice.Columns.Add("type", typeof(string));

            foreach (DataRow row in notice.Rows)
            {
                row["type"] = "notice";
            }

            if (notice.Rows.Count == 0)
            {
                dic["success"] = false;
                dic["message"] = "게시글이 없습니다";

            }
            else
            {
                dic["success"] = true;
                dic["notice"] = notice;
            }

            return dic;

        }

        /// <remarks>
        /// 공지사항 리스트
        /// 
        ///     {
        ///         "assIdx" : 1,
        ///         "division" : "region", (region, area, club)
        ///         "type" : "전체",  (전체, 작성자, 제목)
        ///         "search" : "",
        ///         "page" : 1
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Notice")]
        public object Get(string filter)
        {
            Dictionary<string, object> check = new Dictionary<string, object>();

            if (Request.Headers.Authorization == null)
            {
                check["success"] = false;
                check["message"] = "토큰이 없습니다";
                return check;
            }
            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();

            JObject root = JObject.Parse(filter);

            string division = root["division"].ToString();
            int assIdx = Convert.ToInt32(root["assIdx"].ToString());
            string search = root["search"].ToString() == "" ? string.Empty : root["search"].ToString();
            string type = root["type"].ToString();
            int page = Convert.ToInt32(root["page"].ToString());
            int start = ((page - 1) * 25) + 1;
            int end = page * 25;

            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataSet ds = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetNoticeList(division, assIdx, type, search, start, end);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                dic["success"] = false;
                dic["message"] = "게시글이 없습니다";
            }

            dic["success"] = true;
            dic["total"] = ds.Tables[1].Rows[0]["total"];
            dic["data"] = ds.Tables[0];

            return dic;

        }

        /// <remarks>
        /// 공지사항 등록
        /// 
        ///     {
        ///           "assIdx" : 1,
        ///           "title": "공지사항 업로드 테스트",
        ///           "contents": "파일 업로드 테스트 시작",
        ///           "isAll" : 0,
        ///           "isRegion" : 1,
        ///           "isArea" : 1,
        ///           "isClub" : 1,
        ///            "files" : [
        ///            {
        ///                 "no" : 0
        ///                 "fileName" : "",
        ///                 "filePath" : "/uploads/temp_image/"
        ///            }
        ///           ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        /// 
        [Route("Notice")]
        public object Post([FromBody] Notice nt)
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
            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();

            Dictionary<string, object> dic = new Dictionary<string, object>();

            int noticeIdx = 0;
            int result = 0;
            string path = string.Empty;

            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());
         
            string fileName = string.Empty;
            string sourcePath = string.Empty;
            string type = string.Empty;
            string userFolder = "user_" + memberIdx.ToString();

            var dp = new Duplicate();

            int isRegion = nt.isRegion;
            int isArea = nt.isArea;
            int isClub = nt.isClub;

            if (nt.isAll == 1)
            {
                isRegion = 1;
                isArea = 1;
                isClub = 1;
            }

            //게시글 등록
            try
            {
                using (var biz = new Api_Biz())
                {
                    noticeIdx = biz.SetNotice(memberIdx, nt.assIdx, nt.title, nt.contents, isRegion, isArea, isClub);
                }
            }
            catch (Exception exp)
            {

                dic["success"] = false;
                dic["message"] = exp.ToString();
                return dic;
            }

            if (noticeIdx == -99)
            {
                dic["success"] = false;
                dic["message"] = "datatabase error";
                return dic;
            }


            DataTable files = new DataTable();
            files.Columns.Add("fileName", typeof(string));
            files.Columns.Add("filePath", typeof(string));

            int count = 0; 
            // File 처리
            if (nt.files.Length > 0)
            {
                for (int i = 0; i < nt.files.Length; i++)
                {
                    //filePath : /uploads/temp_file/user_3/
                    fileName = nt.files[i].fileName;
                    sourcePath = HttpContext.Current.Server.MapPath(nt.files[i].filePath.Replace("/", "\\") + nt.files[i].fileName);
                    path = HttpContext.Current.Server.MapPath("~/uploads/file/");

                    if (count == 0)
                    {
                        //폴더 체크 없으면 생성
                        DirectoryInfo di = new DirectoryInfo(path + userFolder);
                        if (!di.Exists)
                        {
                            di.Create();
                        }
                    }
                    path = path + "/" + userFolder;

                    fileName = dp.FileUploadName(path, fileName);

                    path = path + "/" + fileName;

                    //파일이동
                    File.Copy(sourcePath, path);

                    path = "/uploads/file/" + userFolder +"/";

                    DataRow row = files.NewRow();
                    row["fileName"] = fileName;
                    row["filePath"] = path;

                    files.Rows.InsertAt(row, i);
                    
                    count++;
                }

                try
                {
                    using (var biz = new Api_Biz())
                    {
                        type = "notice";
                        result = biz.UploadFile(type, noticeIdx, files);
                    }
                }
                catch (Exception e)
                {
                    dic["success"] = false;
                    dic["message"] = "처리중 오류(file)";
                    return dic;
                }
                if (result == -99 || result == -1 || result == -2)
                {
                    dic["success"] = false;
                    dic["message"] = "처리중 오류(file)";
                    return dic;
                }
                
            }


            if (result == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상처리";
                dic["noticeIdx"] = noticeIdx;


            }
            else
            {
                dic["success"] = false;
                dic["message"] = "처리중 오류";
            }

            return dic;
        }

        /// <remarks>
        /// 공지사항 수정
        /// 
        ///     {   
        ///           "noticeIdx" : 1,
        ///           "title": "공지사항 업로드 테스트",
        ///           "contents": "파일 업로드 테스트 시작",
        ///            "editFiles" : [
        ///            {
        ///                 "no" : 1,
        ///                 "fileName" : "",
        ///                 "filePath" : "/uploads/temp_image/"
        ///            }
        ///           ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("Notice")]
        public object Patch([FromBody] Notice nt)
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

            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();
            Dictionary<string, object> dic = new Dictionary<string, object>();

            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());


            string fileName = string.Empty;
            string sourcePath = string.Empty;
            string type = string.Empty;
            string path = string.Empty;

            int result = 0;
            DataTable files = new DataTable();
            files.Columns.Add("no", typeof(int));
            files.Columns.Add("fileName", typeof(string));
            files.Columns.Add("filePath", typeof(string));

            var dp = new Duplicate();

            string userFolder = "user_" + memberIdx.ToString();

            int count = 0;
            // File 처리
            if (nt.editFiles.Length > 0)
            {
                for (int i = 0; i < nt.editFiles.Length; i++)
                {
                    //파일추가
                    if (nt.editFiles[i].fileName != "")
                    {
                        fileName = nt.editFiles[i].fileName;
                        sourcePath = HttpContext.Current.Server.MapPath(nt.editFiles[i].filePath.Replace("/", "\\") + nt.editFiles[i].fileName);
                        path = HttpContext.Current.Server.MapPath("/uploads/file/");

                        if (count == 0)
                        {
                            //폴더 체크 없으면 생성
                            DirectoryInfo di = new DirectoryInfo(path + userFolder);
                            if (!di.Exists)
                            {
                                di.Create();
                            }
                        }

                        path = path + "/" + userFolder;

                        fileName = dp.FileUploadName(path, fileName);

                        path = path + "/" + fileName;

                        //파일이동
                        File.Copy(sourcePath, path);

                        path = "/uploads/file/" + userFolder + "/";

                        DataRow row = files.NewRow();
                        row["no"] = nt.editFiles[i].no;
                        row["fileName"] = fileName;
                        row["filePath"] = path;

                        files.Rows.InsertAt(row, i);

                        count++;
                    }
                    //이미지 삭제
                    else if (nt.editFiles[i].fileName == string.Empty)
                    {
                        DataRow row = files.NewRow();
                        row["no"] = nt.editFiles[i].no;
                        row["fileName"] = fileName;

                        path = "/uploads/file/" + userFolder + "/";
                        row["filePath"] = path;
                        files.Rows.InsertAt(row, i);
                    }

                    count++;
                }
            }
            try
            {
                using (var biz = new Api_Biz())
                {
                    //파일수정
                    result = biz.EditFiles("notice", nt.noticeIdx, files);
                }
            }
            catch (Exception exp)
            {

                dic["success"] = false;
                dic["message"] = "처리중 오류(file)";
                return dic;
            }

            if (result == -99 || result == -1 || result == -2)
            {
                dic["success"] = false;
                dic["message"] = "처리중 오류(DB)";
                return dic;
            }


            int isRegion = nt.isRegion;
            int isArea = nt.isArea;
            int isClub = nt.isClub;

            if (nt.isAll == 1)
            {
                isRegion = 1;
                isArea = 1;
                isClub = 1;
            }

            try
            {
                using (var biz = new Api_Biz())
                {
                    //파일수정
                    result = biz.EditNotice(nt.noticeIdx, memberIdx, nt.title, nt.contents, isRegion, isArea, isClub);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if (result == -90)
            {
                dic["success"] = false;
                dic["message"] = "작성자와 일치하지 않습니다.";
            }
            else if (result == 0)
            {
                dic["success"] = true;
                dic["message"] = "정상처리 되었습니다.";
            }
            else
            {
                dic["success"] = false;
                dic["message"] = "db error";
            }
            return dic;
        }


        /// <remarks>
        /// 공지사항 삭제
        /// 
        ///     {   
        ///           "noticeIdx" : 1,
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        /// 

        [Route("Notice/{id}")]
        public object Delete(int id)
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

            //token 정보 조회
            jToken = Request.Headers.Authorization.ToString();
            Dictionary<string, object> dic = new Dictionary<string, object>();

            int memberIdx = Convert.ToInt32(clsJWT.isValidToken(jToken).Payload["memberIdx"].ToString());

            int result = 0;
            try
            {
                using (var biz = new Api_Biz())
                {
                    result = biz.DeleteNotice(id, memberIdx);
                }
            }
            catch (Exception e)
            {

                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;
            }

            if(result == -90)
            {
                dic["success"] = false;
                dic["message"] = "작성자와 수정자가 다릅니다";
                return dic;
            }

            dic["success"] = true;
            dic["message"] = "정상처리 되었습니다";
            return dic;
                
        }
    }
}

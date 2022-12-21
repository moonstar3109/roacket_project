using ADMIN.API.DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ClubMemberGradeController : ApiController
    {
        /// <remarks>
        /// 클럽 회원 등급 리스트 조회
        /// 
        ///     {
        ///         
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClubMemberGrade")]
        public object GetClubMemberGradeList()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            DataTable dt = null;

            try
            {
                using (var biz = new Api_Biz())
                {
                    dt = biz.GetClubMemberGradeList();
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

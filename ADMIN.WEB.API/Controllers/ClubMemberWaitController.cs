using ADMIN.API.DA;
using ADMIN.WEB.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ADMIN.WEB.API.Controllers
{
    public class ClubMemberWaitController : ApiController
    {
        /// <remarks>
        /// 클럽가입대기멤버리스트
        /// 클럽 id
        ///     {
        ///         "id" : 0
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClubMemberWait/{id}")]
        public object Get(int id)
        {
            DataSet ds = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    ds = biz.GetClubMemberWaitList(id);
                }
            }
            catch (Exception e)
            {
                dic["success"] = false;
                dic["message"] = e.ToString();
                return dic;

            }

            dic["success"] = true;
            dic["data"] = ds.Tables[0];

            return dic;
        }


        /// <remarks>
        /// 클럽가입대기멤버승인 [대기id = waitIdx]
        /// 
        ///     {
        ///         "waitIdx" : 0,
        ///         "optionCode" : 0,
        ///         "isAccept" : true
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClubMemberWait")]
        public object Post([FromBody] ClubMemberWait cmw)
        {
            int rtn = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using(var biz = new Api_Biz())
                {
                    rtn = biz.ConfirmWaitClubMember(cmw.waitIdx, cmw.optionCode, 1,  true);
                }
            }
            catch (Exception exp)
            {
                dic["success"] = false;
                dic["message"] = exp.ToString();
                return dic;
            }

            dic["success"] = true;
            dic["message"] = rtn;
            return dic;
        }

        /// <remarks>
        /// 클럽가입대기멤버거부 [대기id = waitIdx]
        /// 
        ///     {
        ///         "waitIdx" : 0,
        ///         "rejectReason" : "가입 거부의 사유가 들어갑니다",
        ///         "isAccept" : true
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [Route("ClubMemberWait")]
        public object Put([FromBody] ClubMemberWait cmw)
        {
            int rtn = 0;
            Dictionary<string, object> dic = new Dictionary<string, object>();

            try
            {
                using (var biz = new Api_Biz())
                {
                    rtn = biz.RejectWaitClubMember(cmw.waitIdx, cmw.rejectReason, 1, false);
                }
            }
            catch (Exception exp)
            {
                dic["success"] = false;
                dic["message"] = exp.ToString();
                return dic;
            }

            dic["success"] = true;
            dic["message"] = rtn;
            return dic;
        }
    }
}
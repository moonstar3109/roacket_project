using Microsoft.Practices.EnterpriseLibrary.Data;
using Statnco.FW.Dac;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADMIN.API.DA
{
    public class Api_DA : DataAccessBase
    {
        public Api_DA(Database DB) : base(DB) { }
        public Api_DA(Database DB, DbTransaction DBTrans) : base(DB, DBTrans) { }

        #region 권한확인
        public DataTable GetAdminCheck(int memberIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_MANAGE_NTR");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 로그인
        public DataSet Login(string uid, string password)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_LOGIN_NTR");

            DB.AddInParameter(cmd, "@pMEMBER_ID", DbType.String, uid);
            DB.AddInParameter(cmd, "@pPASS_WD", DbType.String, password);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 소셜로그인

        #region 어드민소셜로그인
        public DataTable GetAdminSocialLogin(string id, string type)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_SNS_LOGIN_NTR");

            DB.AddInParameter(cmd, "@pID", DbType.String, id);
            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 회원조회
        public DataTable GetMember(string id, string joinType)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ADMIN_MEMBER_NTR");

            DB.AddInParameter(cmd, "@pMEMBER_ID", DbType.String, id);
            DB.AddInParameter(cmd, "@pJOIN_TYPE", DbType.String, joinType);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 임시회원등록
        public int SetTempMember(string id, string joinType)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ADMIN_TEMP_MEMBER_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_ID", DbType.String, id);
            DB.AddInParameter(cmd, "@pJOIN_TYPE", DbType.String, joinType);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        

        #region 코드

        #region 급수

        #region 시군구 급수수정
        public int SetAreaClass(int assIdx, DataTable areaList, DataTable classList)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_AREA_CLASS_EDIT_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            SqlParameter param = new SqlParameter("@pAREA_LIST", areaList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            SqlParameter param2 = new SqlParameter("@pCLASS_LIST", classList);
            param2.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param2);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 시도 급수수정
        public int EditRegionClass(int assIdx, DataTable editList)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLASS_EDIT_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            SqlParameter param = new SqlParameter("@pEDIT_LIST", editList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 시군구 급수조회
        public DataSet GetAreaClassList(string type, int idx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_AREA_CLASS_LIST_NTR");

            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 급수조회
        public DataSet GetClassList(string type, int idx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_CLASS_LIST_NTR");

            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);


            return ExecuteDataSet(cmd);
        }
        #endregion

        #endregion

        #region 옷
        public DataTable GetDressList()
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_DRESS_LIST_NTR");

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 신발
        public DataTable GetShoesList()
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_SHOES_LIST_NTR");

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 클럽 회원 등급
        public DataTable GetClubMemberGradeList()
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_CLUB_MEMBER_GRADE_LIST_NTR");

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 사유
        public DataTable GetReasonList()
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_REASON_LIST_NTR");

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #endregion

        #region 관리

        #region 관리목록
        public DataSet GetManageList(int memberIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_ADMIN_MANAGE_LIST_NTR");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 관리상세조회
        public DataSet GetManageDetail(int memberIdx, string type, int idx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_ADMIN_MANAGE_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 클럽정보
        public DataSet GetClubInfo(int clubIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ADMIN_CLUB_INFO_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #endregion

        #region 이적

        #region 이적신청
        public int ReqTransfer(int memberIdx, int befRegionCode, int befAreaCode, int clubIdx,
                               int aftRegionCode, int aftAreaCode, int tClubIdx, string filePath, string reason, int reqMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_TRANSFER_REQ_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pBEF_REGION_CODE", DbType.Int32, befRegionCode);
            DB.AddInParameter(cmd, "@pBEF_AREA_CODE", DbType.Int32, befAreaCode);
            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pAFT_REGION_CODE", DbType.Int32, aftRegionCode);
            DB.AddInParameter(cmd, "@pAFT_AREA_CODE", DbType.Int32, aftAreaCode);
            DB.AddInParameter(cmd, "@pTRANS_CLUB_IDX", DbType.Int32, tClubIdx);
            DB.AddInParameter(cmd, "@pFILE_PATH", DbType.String, filePath);
            DB.AddInParameter(cmd, "@pREASON", DbType.String, reason);
            DB.AddInParameter(cmd, "@pREQ_MEMBER", DbType.Int32, reqMember);


            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 이적 승인/거절
        public int EditTransfer(int transIdx, int editMember, int isOk, string editReason)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_TRANSFER_EDIT_TRX");

            DB.AddInParameter(cmd, "@pTRANS_IDX", DbType.Int32, transIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);
            DB.AddInParameter(cmd, "@pIS_OK", DbType.Int32, isOk);
            DB.AddInParameter(cmd, "@pEDIT_REASON", DbType.String, editReason);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 이적리스트
        public DataTable GetTransferList(int clubIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_TRANSFER_LIST_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 이적상세조회
        public DataSet GetTransferDetail(int transIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_TRANSFER_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pTRANS_IDX", DbType.Int32, transIdx);
            return ExecuteDataSet(cmd);
        }
        #endregion

        #endregion

        #region 협회

        #region 등록을위한 클럽검색
        public DataSet GetAllClub(int assIdx, int regionCode, int option, string search)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ALL_CLUB_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 협회 소속 클럽 상세정보
        public DataSet GetAssClubInfo(int clubIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_CLUB_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 협회 소속 클럽

        #region 클럽 조회
        public DataSet GetAssClub(int assIdx, int option, string search)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_CLUB_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);

            return ExecuteDataSet(cmd);

        }
        #endregion

        #region 클럽 등록
        public int SetClub(int assIdx, DataTable registList, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_CLUB_REGISTER_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            SqlParameter param = new SqlParameter("@pREGIST_LIST", registList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 클럽 해지
        public int CancelClub(int assIdx, int clubIdx, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_CLUB_CANCEL_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 협회 지역리스트 조회
        public DataTable GetAssAreaList(int assIdx, int regionCode)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_AREA_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, regionCode);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 협회 리스트 조회
        public DataTable GetAssList(int regionCode)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_ASS_LIST_NTR");

            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, regionCode);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 협회관리자관리

        #region 협회관리자수정
        public int EditAssMember(int assIdx, int memberIdx, int poCode, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_MEMBER_EDIT_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pPO_CODE", DbType.Int32, poCode);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 협회관리자해제(삭제처리)
        public int DeleteAssMember(int assIdx, DataTable delList, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_MEMBER_DEL_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            SqlParameter param = new SqlParameter("@pDEL_MEMBER", delList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 협회관리자등록
        public int SetAssAdmin(int assIdx, int memberIdx, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_MEMBER_SET_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 협회관리자목록
        public DataSet GetAdminMemberList(int assIdx, int option, string search)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_MEMBER_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 협회관리자상세
        public DataTable GetdAssAdminMemberDetail(int assIdx, int memberIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_MEMBER_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 협회관리자등록을위한검색
        public DataSet GetAllAdminMember(int assIdx, int option, string search)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_MEMBER_SEARCH_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 협회회원관리

        #region 회원급수 비교
        public DataTable CheckMemberClassCompare(int assType, int editMember, DataTable checkList)
        {

            DbCommand cmd = DB.GetStoredProcCommand("ASP_MEMBER_CLASS_COMPARE_TRX");

            DB.AddInParameter(cmd, "@pASS_TYPE", DbType.Int32, assType);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);
            SqlParameter param = new SqlParameter("@pCLASS_LIST", checkList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 엑셀 회원추가
        public DataTable AddMemberToExcel(int assType, DataTable memberList, int editMember)
        {

            DbCommand cmd = DB.GetStoredProcCommand("ASP_MEMBER_ADD_EXCEL_TRX");

            DB.AddInParameter(cmd, "@pASS_TYPE", DbType.Int32, assType);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);
            SqlParameter param = new SqlParameter("@pMEMBER_LIST", memberList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);


            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 엑셀 클럽추가
        public DataTable AddClubToExcel( int assIdx, DataTable clubList, int editMember)
        {

            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_ADD_EXCEL_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);
            SqlParameter param = new SqlParameter("@pCLUB_LIST", clubList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);


            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 협회회원전체조회
        public DataSet GetAssAllMember(int assIdx, int region, int area, int option, int option2, string search, int start, int end)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ALL_MEMBER_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, region);
            DB.AddInParameter(cmd, "@pAREA", DbType.Int32, area);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pOPTION2", DbType.Int32, option2);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);
            DB.AddInParameter(cmd, "@pSTART", DbType.Int32, start);
            DB.AddInParameter(cmd, "@pEND", DbType.Int32, end);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 협회회원 급수 인증
        public int SetMemberClassAuth(int assIdx, DataTable classList, int editMember)
        {

            DbCommand cmd = DB.GetStoredProcCommand("ASP_MEMBER_CLASS_AUTH_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);
            SqlParameter param = new SqlParameter("@pCLASS_LIST", classList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 협회회원 개별 등록
        public int AddMemberSeperate(int assIdx, int regionCode, DataTable memberList, int editMember)
        {

            DbCommand cmd = DB.GetStoredProcCommand("ASP_MEMBER_REGIST_SEP_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);
            SqlParameter param = new SqlParameter("@pMEMBER_LIST", memberList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 협회회원상세조회
        public DataSet GetAssAllMemberDetail(int clubIdx, int assIdx, int memberIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ALL_MEMBER_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 협회회원수정
        public int EditAssAllMemberInfo(int memberIdx, int clubIdx, string birth, string phone, string gender,
                                        int assIdx, int regionClassCode, int areaClassCode, string reason,
                                        int dressCode, int shoesCode, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ALL_MEMBER_EDIT_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pBIRTH", DbType.String, birth);
            DB.AddInParameter(cmd, "@pPHONE", DbType.String, phone);
            DB.AddInParameter(cmd, "@pGENDER", DbType.String, gender);

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pREGION_CLASS_CODE", DbType.Int32, regionClassCode);
            DB.AddInParameter(cmd, "@pAREA_CLASS_CODE", DbType.Int32, areaClassCode);
            DB.AddInParameter(cmd, "@pREASON", DbType.String, reason);

            DB.AddInParameter(cmd, "@pDRESS_CODE", DbType.Int32, dressCode);
            DB.AddInParameter(cmd, "@pSHOES_CODE", DbType.Int32, shoesCode);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #endregion

        #region 직위, 등급

        #region 등급조회
        public DataTable GetAssGrade(int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_GRADE_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }

        #region 직위조회
        public DataTable GetAssPosition(int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_POSITION_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }

        #endregion

        #region 직위추가
        public int SetAssPosition(int assIdx, DataTable positions)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_POSITION_ADD_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            SqlParameter param = new SqlParameter("@pPOSITION_LIST", positions);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 직위수정
        public int EditAssPosition(int assIdx, int poCode, string position, int assManage, int noticeManage, int clubManage, int memberManage, int transferManage)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_POSITION_EDIT_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pPO_CODE", DbType.Int32, poCode);
            DB.AddInParameter(cmd, "@pPOSITION", DbType.String, position);
            DB.AddInParameter(cmd, "@pASS_MANAGE", DbType.Int32, assManage);
            DB.AddInParameter(cmd, "@pNOTICE_MANAGE", DbType.Int32, noticeManage);
            DB.AddInParameter(cmd, "@pCLUB_MANAGE", DbType.Int32, clubManage);
            DB.AddInParameter(cmd, "@pMEMBER_MANAGE", DbType.Int32, memberManage);
            DB.AddInParameter(cmd, "@pTRANSFER_MANAGE", DbType.Int32, transferManage);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 직위삭제
        public int DelAssPosition(int assIdx, int poCode)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_ADMIN_POSITION_DEL_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pPO_CODE", DbType.Int32, poCode);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 가입대기

        #region 대기목록 조회
        public DataTable GetWaitClubList(int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_ASS_CLUB_WAIT_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 가입 승인/거절/취소 처리
        public int SetAssClubWait(int assIdx, int clubIdx, int isOk, int editMember, string reason)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_CLUB_WAIT_EDIT_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pIS_OK", DbType.Int32, isOk);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);
            DB.AddInParameter(cmd, "@pREASON", DbType.String, reason);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 클럽수정

        #region 클럽명 변경 목록
        public DataSet GetClubEditList(int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_NAME_EDIT_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 수정클럽 상세정보
        public DataTable GetClubEditDetail(int editIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_NAME_EDIT_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pEDIT_IDX", DbType.Int32, editIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 클럽명 변경 승인/거절
        public int ClubNameEdit(int editIdx, int isOk, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_NAME_EDIT_TRX");

            DB.AddInParameter(cmd, "@pEDIT_IDX", DbType.Int32, editIdx);
            DB.AddInParameter(cmd, "@pIS_OK", DbType.Int32, isOk);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 클럽명 변경 신청
        public int ReqClubNameChange(int clubIdx, string befClubName, string aftClubName, int reqMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_NAME_EDIT_REQ_TRX");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pBEF_CLUB_NAME", DbType.String, befClubName);
            DB.AddInParameter(cmd, "@pAFT_CLUB_NAME", DbType.String, aftClubName);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, reqMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 이적

        #region 이적리스트
        public DataSet GetAssTransferList(int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_ASS_TRANSFER_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 이적 취소 처리
        public int CancelTransfer(int transIdx, string cancelReason, int editMember, int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_TRANSFER_CANCEL_TRX");

            DB.AddInParameter(cmd, "@pTRANS_IDX", DbType.Int32, transIdx);
            DB.AddInParameter(cmd, "@pCANCEL_REASON", DbType.String, cancelReason);
            DB.AddInParameter(cmd, "@pCANCEL_MEMBER", DbType.Int32, editMember);
            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 이적 상세보기
        public DataTable GetAssTransferDetail(int transIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_TRANSFER_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pTRANS_IDX", DbType.Int32, transIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #endregion

        

        #endregion

        #region 클럽

        #region 클럽리스트
        public DataTable GetClubList(int clubIdx, int regionCode, int areaCode)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_LIST_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pAREA_CODE", DbType.Int32, areaCode);

            return ExecuteDataSet(cmd).Tables[0];

        }
        #endregion

        #region 클럽정보
        public DataSet GetClub(int clubIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 클럽 수정
        public int EditClub(int clubIdx, int memberIdx, string clubName, int stadiumIdx, string memo)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_EDIT_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pCLUB_NAME", DbType.String, clubName);
            DB.AddInParameter(cmd, "@pSTADIUM_IDX", DbType.Int32, stadiumIdx);
            DB.AddInParameter(cmd, "@pMEMO", DbType.String, memo);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 클럽 회원 리스트
        public DataSet GetClubMemberList(int clubIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_CLUB_MEMBER_LIST_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 클럽 회원 정보
        public DataSet GetClubMemberInfo(int memberIdx, int clubIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_CLUB_MEMBER_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region (클럽)회원수정
        public int EditMemberInfo(int memberIdx, int clubIdx, string birth,string phone ,string gender, int gradeCode, int regionClassCode, int areaClassCode,
                                                string addr1, string addr2, int dressCode, int shoesCode)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_MEMBER_EDIT_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pBIRTH", DbType.String, birth);
            DB.AddInParameter(cmd, "@pPHONE", DbType.String, phone);
            DB.AddInParameter(cmd, "@pGENDER", DbType.String, gender);
            DB.AddInParameter(cmd, "@pGRADE_CODE", DbType.Int32, gradeCode);
            DB.AddInParameter(cmd, "@pREGION_CLASS_CODE", DbType.Int32, regionClassCode);
            DB.AddInParameter(cmd, "@pAREA_CLASS_CODE", DbType.Int32, areaClassCode);
            DB.AddInParameter(cmd, "@pADDR1", DbType.String, addr1);
            DB.AddInParameter(cmd, "@pADDR2", DbType.String, addr2);
            DB.AddInParameter(cmd, "@pDRESS_CODE", DbType.Int32, dressCode);
            DB.AddInParameter(cmd, "@pSHOES_CODE", DbType.Int32, shoesCode);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 가입 대기 리스트 
        public DataSet GetClubMemberWaitList(int clubIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_CLUB_MEMBER_WAIT_LIST_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 가입 승인
        public int ConfirmWaitClubMember(int waitIdx, int optionCode, int editIdx, bool isAccept)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_CLUB_MEMBER_WAIT_PROCESS_TRX");

            DB.AddInParameter(cmd, "@pWAIT_IDX", DbType.Int32, waitIdx);
            DB.AddInParameter(cmd, "@pOPTION_CODE", DbType.Int32, optionCode);
            DB.AddInParameter(cmd, "@pEDIT_IDX", DbType.Int32, editIdx);
            DB.AddInParameter(cmd, "@pIS_ACCEPT", DbType.Boolean, isAccept);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 가입 거절
        public int RejectWaitClubMember(int waitIdx, string rejectReason, int editIdx, bool isAccept)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_CLUB_MEMBER_WAIT_PROCESS_TRX");

            DB.AddInParameter(cmd, "@pWAIT_IDX", DbType.Int32, waitIdx);
            DB.AddInParameter(cmd, "@pREJECT_REASON", DbType.String, rejectReason);
            DB.AddInParameter(cmd, "@pEDIT_IDX", DbType.Int32, editIdx);
            DB.AddInParameter(cmd, "@pIS_ACCEPT", DbType.Boolean, isAccept);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 클럽 회원 추가
        public int SetClubMember(DataTable members, int editIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_CLUB_MEMBER_TRX");

            SqlParameter param = new SqlParameter("@pMEMBER_LIST", members);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);
            DB.AddInParameter(cmd, "@pEDIT_IDX", DbType.Int32, editIdx);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 클럽 회원(복수) 급수 변경
        public int SetClubMembersClasses(DataTable regionClassCode, DataTable areaClassCode, int editIdx) 
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_MEMBERS_CLASSES_TRX");

            SqlParameter param = new SqlParameter("@pREGION_CLASS_CODE_TABLE", regionClassCode);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            SqlParameter param2 = new SqlParameter("@pAREA_CLASS_CODE_TABLE", areaClassCode);
            param2.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param2);

            DB.AddInParameter(cmd, "@pEDIT_IDX", DbType.Int32, editIdx);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }

        #endregion

        #region 클럽 회원 급수 조회
        public DataSet GetClubMembersClasses(int assIdx, int region, int area, int option, string search)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_MEMBER_CLASS_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, region);
            DB.AddInParameter(cmd, "@pAREA", DbType.Int32, area);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 클럽 회원 급수 수정
        public int EditClubMembersClasses(int assIdx, int assType, string reason, DataTable classList, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_MEMBERS_EDIT_CLASSES_TRX");
            
            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pASS_TYPE", DbType.Int32, assType);
            DB.AddInParameter(cmd, "@pREASON", DbType.String, reason);
            SqlParameter param = new SqlParameter("@pEDIT_LIST", classList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);
            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }

        #endregion

        #region 제명

        #region 제명 조회
        public DataTable GetExpulsion(string type, int idx, int memberIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_EXPULSION_NTR");

            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion  

        #region 제명변경
        public int EditExpulsion(int expIdx, DateTime srtDate, DateTime endDate, Boolean isExp, string expReason, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_EXPULSION_EDIT_TRX");

            DB.AddInParameter(cmd, "@pEXP_IDX", DbType.Int32, expIdx);
            DB.AddInParameter(cmd, "@pSRT_DATE", DbType.DateTime, srtDate);
            DB.AddInParameter(cmd, "@pEND_DATE", DbType.DateTime, endDate);
            DB.AddInParameter(cmd, "@pIS_EXP", DbType.Boolean, isExp);
            DB.AddInParameter(cmd, "@pEXP_REASON", DbType.String, expReason);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 제명취소
        public int CancelExpulsion(int expIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_EXPULSION_DEL_TRX");

            DB.AddInParameter(cmd, "@pEXP_IDX", DbType.Int32, expIdx);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 제명추가
        public int SetExpulsion(string type, int idx, int memberIdx, DateTime startDate, DateTime endDate, string expReason, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_EXPULSION_ADD_TRX");

            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pSTART_DATE", DbType.DateTime, startDate);
            DB.AddInParameter(cmd, "@pEND_DATE", DbType.DateTime, endDate);
            DB.AddInParameter(cmd, "@pEXP_REASON", DbType.String, expReason);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);


            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #endregion

        #region 공통

        #region 협회명 반환
        public DataTable GetAssName(int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASSOCIATION_NAME_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region  임원진 정보 모달
        public DataTable GetAdminInfoToModal(int memberIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ADMIN_INFO_MODAL_NTR");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 엑셀

        #region 급수비교양식다운로드
        public DataTable GetClassExcelForm(string type, int idx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_MEMBER_CLASS_COMPARE_FORM_NTR");

            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #endregion

        #region 클럽 등록시 검색 
        public DataSet GetAssClubSearch(int regionCode, int option, int areaCode, string search)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_ASS_CLUB_SEARCH_NTR");
            
            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pAREA_CODE", DbType.Int32, areaCode);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 전체회원 검색
        public DataSet GetAllMember(int assIdx, int options, string search)
        {

            DbCommand cmd = DB.GetStoredProcCommand("ASP_ALL_MEMBER_LIST_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, options);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 시도
        public DataTable GetRegion()
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_REGION_NTR");

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 시군구
        public DataTable GetArea(int regionCode)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_AREA_NTR");

            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, regionCode);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 클럽명 중복확인
        public int ClubNameOverlap(int region, int area, string clubName)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_CLUB_NAME_OVERLAP_CHECK");

            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, region);
            DB.AddInParameter(cmd, "@pAREA_CODE", DbType.Int32, area);
            DB.AddInParameter(cmd, "@pCLUB_NAME", DbType.String, clubName);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 협회명 중복확인
        public int AssNameOverlap(int region, int area, string assName)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_ASS_NAME_OVERLAP_CHECK");

            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, region);
            DB.AddInParameter(cmd, "@pAREA_CODE", DbType.Int32, area);
            DB.AddInParameter(cmd, "@pASS_NAME", DbType.String, assName);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 소속클럽리스트 조회
        public DataTable GetBelongClubList(int region, int area, int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_BELONG_CLUB_LIST_NTR");

            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, region);
            DB.AddInParameter(cmd, "@pAREA", DbType.Int32, area);
            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 미가입회원 동명이인
        public int GetDuplicateUnsingMember(int clubIdx, string memberName, string gender, string birth)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_UNSIGN_MEMBER_DUPLICATE_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pMEMBER_NAME", DbType.String, memberName);
            DB.AddInParameter(cmd, "@pGENDER", DbType.String, gender);
            DB.AddInParameter(cmd, "@pBIRTH", DbType.String, birth);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 업로드

        #region 사진
        public int UploadImage(string type, int idx, DataTable images)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_IMAGE_UPLOAD_TRX");
            
            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);
            SqlParameter param = new SqlParameter("@pIMAGES_TABLE", images);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 파일
        public int UploadFile(string type, int idx, DataTable files)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_FILE_UPLOAD_TRX");

            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);
            SqlParameter param = new SqlParameter("@pFILES_TABLE", files);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 폴더명 가져오기
        public DataTable GetFolderName(string addType, string type, int idx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_FOLDER_NAME_NTR");
            
            DB.AddInParameter(cmd, "@pADD_TYPE", DbType.String, addType);
            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 댓글 폴더명 가져오기
        public DataTable GetReplyFolderName(int repIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_REPLY_FOLDER_NAME_NTR");

            DB.AddInParameter(cmd, "@pREP_IDX", DbType.Int32, repIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region  사진 수정/삭제
        public int EditUpload(string type, int idx, DataTable files)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_EDIT_UPLOAD_TRX");

            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);
            SqlParameter param = new SqlParameter("@pEDIT_TABLE", files);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region  파일 수정/삭제
        public int EditFiles(string type, int idx, DataTable files)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_EDIT_FILES_TRX");

            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);
            SqlParameter param = new SqlParameter("@pEDIT_TABLE", files);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion
        #endregion

        #region 다운로드

        #region 파일목록
        public DataTable GetFileList(string type, int idx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_FILE_LIST_NTR");

            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pIDX", DbType.Int32, idx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #endregion

        #region 공지사항

        #region 공지사항 삭제
        public int DeleteNotice(int noticeIdx, int memberIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_NOTICE_DEL_TRX");

            DB.AddInParameter(cmd, "@pNOTICE_IDX", DbType.Int32, noticeIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, memberIdx);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));

        }
        #endregion

        #region 공지사항 수정
        public int EditNotice(int noticeIdx, int memberIdx, string title, string contents, int isRegion, int isArea, int isClub)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_NOTICE_EDIT_TRX");

            DB.AddInParameter(cmd, "@pNOTICE_IDX", DbType.Int32, noticeIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pTITLE", DbType.String, title);
            DB.AddInParameter(cmd, "@pCONTENTS", DbType.String, contents);
            DB.AddInParameter(cmd, "@pIS_REGION", DbType.Int32, isRegion);
            DB.AddInParameter(cmd, "@pIS_AREA", DbType.Int32, isArea);
            DB.AddInParameter(cmd, "@pIS_CLUB", DbType.Int32, isClub);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 공지사항 등록
        public int SetNotice(int memberIdx, int assIdx, string title, string contents, int isRegion, int isArea, int isClub)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_NOTICE_WRITE_TRX");

            DB.AddInParameter(cmd, "@pWRITER", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pTITLE", DbType.String, title);
            DB.AddInParameter(cmd, "@pCONTENTS", DbType.String, contents);
            DB.AddInParameter(cmd, "@pIS_REGION", DbType.Int32, isRegion);
            DB.AddInParameter(cmd, "@pIS_AREA", DbType.Int32, isArea);
            DB.AddInParameter(cmd, "@pIS_CLUB", DbType.Int32, isClub);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 공지사항 조회 리스트
        public DataSet GetNoticeList(string division, int assIdx, string type, string search, int start, int end)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_NOTICE_LIST_NTR");
            
            DB.AddInParameter(cmd, "@pDIVISION", DbType.String, division);
            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pTYPE", DbType.String, type);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);
            DB.AddInParameter(cmd, "@pSTART", DbType.Int32, start);
            DB.AddInParameter(cmd, "@pEND", DbType.Int32, end);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 공지사항 상세 조회
        public DataSet GetNoticeDetail(int noticeIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_NOTICE_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pNOTICE_IDX", DbType.Int32, noticeIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #endregion

        #region 댓글

        #region 댓글 수정
        public int EditReply(int memberIdx, int noticeIdx, int repIdx, string contents, string filePath)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_REPLY_EDIT_TRX");

            DB.AddInParameter(cmd, "@pWRITER", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pNOTICE_IDX", DbType.Int32, noticeIdx);
            DB.AddInParameter(cmd, "@pREP_IDX", DbType.Int32, repIdx);
            DB.AddInParameter(cmd, "@pCONTENTS", DbType.String, contents);
            DB.AddInParameter(cmd, "@pFILE_PATH", DbType.String, filePath);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 댓글 작성
        public int SetReply(int memberIdx, int noticeIdx, int repIdx, int depth, string contents, string filePath)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_REPLY_WRITE_TRX");

            DB.AddInParameter(cmd, "@pWRITER", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pNOTICE_IDX", DbType.Int32, noticeIdx);
            DB.AddInParameter(cmd, "@pREP_IDX", DbType.Int32, repIdx);
            DB.AddInParameter(cmd, "@pDEPTH", DbType.Int32, depth);
            DB.AddInParameter(cmd, "@pCONTENTS", DbType.String, contents);
            DB.AddInParameter(cmd, "@pFILE_PATH", DbType.String, filePath);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 댓글 조회
        public DataSet GetReplyList(int noticeIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("dbo.ASP_REPLY_NTR");

            DB.AddInParameter(cmd, "@pNOTICE_IDX", DbType.Int32, noticeIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 댓글삭제 
        public int DelReply(int repIdx, int memberIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_REPLY_DEL_TRX");

            DB.AddInParameter(cmd, "@pREP_IDX", DbType.Int32, repIdx);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 경기장

        #region 경기장 조회
        public DataTable GetStadium(int regionCode, int areaCode, string search)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_STADIUM_LIST_NTR");

            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pAREA_CODE", DbType.Int32, areaCode);
            DB.AddInParameter(cmd, "@pSTADIUM_NAME", DbType.String, search);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 경기장 요청
        public int SetStadiumRquest(int memberIdx, int regionCode, int areaCode, string stadiumName, int clubIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ASP_STADIUM_REQUEST_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pAREA_CODE", DbType.Int32, areaCode);
            DB.AddInParameter(cmd, "@pSTADIUM_NAME", DbType.String, stadiumName);
            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 어드민

        #region 관리정보
        public DataSet GetAdminManageDetail()
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_MANAGE_DETAIL_NTR");

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 회원

        #region 회원목록
        public DataSet GetStatncoAdminMemberList(int block, int option, string search, int start, int end)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_MEMBER_LIST_NTR");
            
            DB.AddInParameter(cmd, "@pIS_BLOCK", DbType.Int32, block);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);
            DB.AddInParameter(cmd, "@pSTART", DbType.Int32, start);
            DB.AddInParameter(cmd, "@pEND", DbType.Int32, end);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 회원상세
        public DataSet GetStatncoAdminMemberDetail(int regAssIdx, int areAssIdx, int memberIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_MEMBER_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pREG_ASS_IDX", DbType.Int32, regAssIdx);
            DB.AddInParameter(cmd, "@pARE_ASS_IDX", DbType.Int32, areAssIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 회원탈퇴
        public int DelMember(int memberIdx, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_MEMBER_DEL_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 차단

        #region 회원차단
        public int EditMemberblock(int memberIdx, int type, string memo, string start, string end, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_MEMBER_BLOCK_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pTYPE", DbType.Int32, type);
            DB.AddInParameter(cmd, "@pMEMO", DbType.String, memo);
            DB.AddInParameter(cmd, "@pSTART", DbType.String, start);
            DB.AddInParameter(cmd, "@pEND", DbType.String, end);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 차단변경
        public int editBlock(int memberIdx, int type, string memo, string start, string end, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_MEMBER_BLOCK_EDIT_TRX");

            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pTYPE", DbType.Int32, type);
            DB.AddInParameter(cmd, "@pMEMO", DbType.String, memo);
            DB.AddInParameter(cmd, "@pSTART", DbType.String, start);
            DB.AddInParameter(cmd, "@pEND", DbType.String, end);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 클럽

        #region 클럽목록
        public DataSet GetStatncoAdminClubList(int region, int area, int option, string search, int start, int end)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_CLUB_LIST_NTR");

            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, region);
            DB.AddInParameter(cmd, "@pAREA", DbType.Int32, area);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);
            DB.AddInParameter(cmd, "@pSTART", DbType.Int32, start);
            DB.AddInParameter(cmd, "@pEND", DbType.Int32, end);
            
            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 클럽정보
        public DataSet GetAdminClubInfo(int clubIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_CLUB_INFO_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 클럽회원전체조회
        public DataSet GetAllClubMember(int clubIdx, int option, string search)
        {

            DbCommand cmd = DB.GetStoredProcCommand("ROK_ALL_CLUB_MEMBER_LIST_NTR");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 클럽메모작성
        public int RegistClubMemo(int clubIdx, string memo, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_CLUB_MEMO_WRITE_TRX");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pMEMO", DbType.String, memo);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 클럽폐쇄
        public int DeleteClub(int clubIdx, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_CLUB_CLOSE_TRX");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 클럽장 양도
        public int AssignClubMaster(int clubIdx, int memberIdx, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_CLUB_MASTER_ASSIGN_TRX");

            DB.AddInParameter(cmd, "@pCLUB_IDX", DbType.Int32, clubIdx);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 협회

        #region 협회상세조회
        public DataSet GetStatncoAdminAssociationDetail(int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_ASS_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 협회생성
        public int CreateAssociation(int region, int area, string assName, string assType, int memberIdx, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_ASS_CREATE_TRX");

            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, region);
            DB.AddInParameter(cmd, "@pAREA", DbType.Int32, area);
            DB.AddInParameter(cmd, "@pASS_NAME", DbType.String, assName);
            DB.AddInParameter(cmd, "@pASS_TYPE", DbType.String, assType);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 협회목록
        public DataSet GetStatncoAdminAssociationList(int region, int area, int option, string search, int start, int end)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_ASS_LIST_NTR");

            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, region);
            DB.AddInParameter(cmd, "@pAREA", DbType.Int32, area);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);
            DB.AddInParameter(cmd, "@pSTART", DbType.Int32, start);
            DB.AddInParameter(cmd, "@pEND", DbType.Int32, end);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 협회메모작성
        public int RegistAssMemo(int assIdx, string memo, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_ASS_MEMO_WRITE_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pMEMO", DbType.String, memo);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 협회셋팅조회
        public DataTable GetStatncoAdminAssociationDetailSetting(int assIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_ASS_SETTING_NTR");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 협회셋팅수정
        public int EditStatncoAssocationDetailSetting(int assIdx, int regionCode, int areaCode, string assName, string assType, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_ASS_SETTING_EDIT_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pREGION_CODE", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pAREA_CODE", DbType.Int32, areaCode);
            DB.AddInParameter(cmd, "@pASS_NAME", DbType.String, assName);
            DB.AddInParameter(cmd, "@pASS_TYPE", DbType.String, assType);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 협회마스터양도
        public int AssingAssMaster(int assIdx, int memberIdx, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_ASS_MASTER_ASSIGN_TRX");

            DB.AddInParameter(cmd, "@pASS_IDX", DbType.Int32, assIdx);
            DB.AddInParameter(cmd, "@pMEMBER_IDX", DbType.Int32, memberIdx);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }

        #endregion

        #endregion

        #region 체육관

        #region 체육관 목록
        public DataSet GetStatncoAdminStadiumList(int option, string search, int start, int end)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_STADIUM_LIST_NTR");

            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);
            DB.AddInParameter(cmd, "@pSTART", DbType.Int32, start);
            DB.AddInParameter(cmd, "@pEND", DbType.Int32, end);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 체육관 상세정보
        public DataSet GetStatncoAdminStadiumDetail(int stadiumIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_STADIUM_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pSTADIUM_IDX", DbType.Int32, stadiumIdx);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 체육관 메모 작성
        public int EditStadiumMemo(int stadiumIdx, string memo, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_STADIUM_MEMO_WRITE_TRX");

            DB.AddInParameter(cmd, "@pSTADIUM_IDX", DbType.Int32, stadiumIdx);
            DB.AddInParameter(cmd, "@pMEMO", DbType.String, memo);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 체육관 검색어 수정
        public int EditStadiumSearchList(int stadiumIdx, DataTable searchList)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_STADIUM_SEARCH_LIST_EDIT_TRX");

            DB.AddInParameter(cmd, "@pSTADIUM_IDX", DbType.Int32, stadiumIdx);
            SqlParameter param = new SqlParameter("@pSEARCH_LIST", searchList);
            param.SqlDbType = SqlDbType.Structured;
            cmd.Parameters.Add(param);
            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 체육관 정보 수정
        public int EditStadiumDetail(int stadiumIdx, int regionCode, int areaCode, int type, int status,
                                           string stadiumName, string stadiumAddr, string stadiumAddr2, string phone, string searchList, int isUse, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_STADIUM_DETAIL_EDIT_TRX");

            DB.AddInParameter(cmd, "@pSTADIUM_IDX", DbType.Int32, stadiumIdx);
            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pAREA", DbType.Int32, areaCode);
            DB.AddInParameter(cmd, "@pTYPE", DbType.Int32, type);
            DB.AddInParameter(cmd, "@pSTATUS", DbType.Int32, status);
            DB.AddInParameter(cmd, "@pSTADIUM_NAME", DbType.String, stadiumName);
            DB.AddInParameter(cmd, "@pSTADIUM_ADDR", DbType.String, stadiumAddr);
            DB.AddInParameter(cmd, "@pSTADIUM_ADDR2", DbType.String, stadiumAddr2);
            DB.AddInParameter(cmd, "@pPHONE", DbType.String, phone);
            DB.AddInParameter(cmd, "@pTAG", DbType.String, searchList);
            DB.AddInParameter(cmd, "@pIS_USE", DbType.String, isUse);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 체육관 요청 목록
        public DataSet GetStatncoAdminStadiumRequestList(int option, string search, int start, int end)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_STADIUM_REQUEST_LIST_NTR");

            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);
            DB.AddInParameter(cmd, "@pSTART", DbType.Int32, start);
            DB.AddInParameter(cmd, "@pEND", DbType.Int32, end);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #region 체육관 상세정보
        public DataTable GetStatncoAdminStadiumRequestDetail(int requestIdx)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_STADIUM_REQUEST_DETAIL_NTR");

            DB.AddInParameter(cmd, "@pREQUEST_IDX", DbType.Int32, requestIdx);

            return ExecuteDataSet(cmd).Tables[0];
        }
        #endregion

        #region 체육관 등록
        public int CreateStadium(int regionCode, int areaCode, int type, int status,
                                  string stadiumName, string stadiumAddr, string stadiumAddr2, string phone, string searchList, int isUse, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_STADIUM_REGIST_TRX");

            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pAREA", DbType.Int32, areaCode);
            DB.AddInParameter(cmd, "@pTYPE", DbType.Int32, type);
            DB.AddInParameter(cmd, "@pSTATUS", DbType.Int32, status);
            DB.AddInParameter(cmd, "@pSTADIUM_NAME", DbType.String, stadiumName);
            DB.AddInParameter(cmd, "@pSTADIUM_ADDR", DbType.String, stadiumAddr);
            DB.AddInParameter(cmd, "@pSTADIUM_ADDR2", DbType.String, stadiumAddr2);
            DB.AddInParameter(cmd, "@pPHONE", DbType.String, phone);
            DB.AddInParameter(cmd, "@pTAG", DbType.String, searchList);
            DB.AddInParameter(cmd, "@pIS_USE", DbType.Int32, isUse);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #region 체육관 요청처리
        public int SetStatncoAdminStadiumRequestEdit(int requestIdx, int regionCode, int areaCode, int type,
                                                     int status, string stadiumName, string stadiumAddr, string stadiumAddr2, string phone,
                                                     string tag, int isUse, int editMember)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_STADIUM_REQUEST_EDIT_TRX");

            DB.AddInParameter(cmd, "@pREQUEST_IDX", DbType.Int32, requestIdx);
            DB.AddInParameter(cmd, "@pREGION", DbType.Int32, regionCode);
            DB.AddInParameter(cmd, "@pAREA", DbType.Int32, areaCode);
            DB.AddInParameter(cmd, "@pTYPE", DbType.Int32, type);
            DB.AddInParameter(cmd, "@pSTATUS", DbType.Int32, status);
            DB.AddInParameter(cmd, "@pSTADIUM_NAME", DbType.String, stadiumName);
            DB.AddInParameter(cmd, "@pSTADIUM_ADDR", DbType.String, stadiumAddr);
            DB.AddInParameter(cmd, "@pSTADIUM_ADDR2", DbType.String, stadiumAddr2);
            DB.AddInParameter(cmd, "@pPHONE", DbType.String, phone);
            DB.AddInParameter(cmd, "@pTAG", DbType.String, tag);
            DB.AddInParameter(cmd, "@pIS_USE", DbType.Int32, isUse);
            DB.AddInParameter(cmd, "@pEDIT_MEMBER", DbType.Int32, editMember);

            DB.AddOutParameter(cmd, "@oRETURN_NO", DbType.Int32, 4);

            ExecuteNonQuery(cmd);

            return Convert.ToInt32(DB.GetParameterValue(cmd, "@oRETURN_NO"));
        }
        #endregion

        #endregion

        #region 커뮤니티

        #region 게시글 조회
        public DataSet GetStatncoAdminBoardList(string boardType, int option, string search, int start, int end)
        {
            DbCommand cmd = DB.GetStoredProcCommand("ROK_COMUNITY_NTR");

            DB.AddInParameter(cmd, "@pBOARD_TYPE", DbType.String, boardType);
            DB.AddInParameter(cmd, "@pOPTION", DbType.Int32, option);
            DB.AddInParameter(cmd, "@pSEARCH", DbType.String, search);
            DB.AddInParameter(cmd, "@pSTART", DbType.Int32, start);
            DB.AddInParameter(cmd, "@pEND", DbType.Int32, end);

            return ExecuteDataSet(cmd);
        }
        #endregion

        #endregion

        #endregion
        
        #endregion

    }
}

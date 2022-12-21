using Statnco.FW.Dac;
using System;
using System.Data;

namespace ADMIN.API.DA
{
    public class Api_Biz : BizLogicBase
    {
        public Api_Biz() : base("roacket_DB", true) { }

       

        #region 권한

       

        #region 권한확인
        public DataTable GetAdminCheck(int memberIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetAdminCheck(memberIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }

        #endregion

        #endregion

        #region 로그인
        public DataSet Login(string uid, string password)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.Login(uid, password);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 소셜로그인

        #region 어드민소셜로그인
        public DataTable GetAdminSocialLogin(string id, string type)
        {
            DataTable ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAdminSocialLogin(id, type);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 회원조회
        public DataTable GetMember(string id, string joinType)
        {
            DataTable ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetMember(id, joinType);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 임시회원등록
        public int SetTempMember(string id, string joinType)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetTempMember(id, joinType);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 코드

        #region 급수

        #region 시군구 급수수정
        public int SetAreaClass(int assIdx, DataTable areaList, DataTable classList)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetAreaClass(assIdx, areaList, classList);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 시도 급수수정
        public int EditRegionClass(int assIdx, DataTable editList)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditRegionClass(assIdx, editList);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 급수조회
        public DataSet GetClassList(string type, int idx)
        {
            DataSet ds = null;

            try
            {
                using(var da = new Api_DA(DB))
                {
                    ds = da.GetClassList(type, idx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 시군구 급수 조회
        public DataSet GetAreaClassList(string type, int idx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAreaClassList(type, idx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #endregion

        #region 옷
        public DataTable GetDressList()
        {
            DataTable dt = null;

            try
            {
                using(var da = new Api_DA(DB))
                {
                    dt = da.GetDressList();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 신발
        public DataTable GetShoesList()
        {
            DataTable dt = null;

            try
            {
                using(var da = new Api_DA(DB))
                {
                    dt = da.GetShoesList();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 클럽 회원 등급
        public DataTable GetClubMemberGradeList()
        {
            DataTable dt = null;

            try
            {
                using(var da = new Api_DA(DB))
                {
                    dt = da.GetClubMemberGradeList();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 사유
        public DataTable GetReasonList()
        {
            DataTable dt = null;

            try
            {
                using(var da = new Api_DA(DB))
                {
                    dt = da.GetReasonList();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #endregion

        #region 협회

        #region 협회 소속 클럽 상세정보
        public DataSet GetAssClubInfo(int clubIdx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAssClubInfo(clubIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 등록을위한 클럽검색
        public DataSet GetAllClub(int assIdx, int regionCode, int option, string search)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAllClub( assIdx, regionCode, option, search);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 협회 소속 클럽

        #region 클럽 조회
        public DataSet GetAssClub(int assIdx, int option, string search)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAssClub(assIdx, option, search);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;

        }
        #endregion

        #region 클럽등록
        public int SetClub(int assIdx, DataTable registList, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetClub(assIdx, registList, editMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 클럽해지
        public int CancelClub(int assIdx, int clubIdx, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.CancelClub(assIdx, clubIdx, editMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 협회 지역리스트 조회
        public DataTable GetAssAreaList(int assIdx, int regionCode)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetAssAreaList(assIdx, regionCode);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 협회 리스트 조회
        public DataTable GetAssList(int regionCode)
        {
            DataTable dt = null;

            try
            {
                using(var biz = new Api_DA(DB))
                {
                    dt = biz.GetAssList(regionCode);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 협회관리자관리

        #region 협회 관리자 수정
        public int EditAssMember(int assIdx, int memberIdx, int poCode, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditAssMember(assIdx, memberIdx, poCode, editMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;

        }
        #endregion

        #region 협회관리자해제(삭제처리)
        public int DeleteAssMember(int assIdx, DataTable delList, int editMember)
        {
            int result = 0;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.DeleteAssMember(assIdx, delList, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }

        #endregion

        #region 협회관리자등록
        public int SetAssAdmin(int assIdx, int memberIdx, int editMember)
        {
            int result = 0;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetAssAdmin(assIdx, memberIdx, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 협회관리자목록
        public DataSet GetAdminMemberList(int assIdx,  int option, string search)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAdminMemberList(assIdx, option, search);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 협회관리자상세조회
        public DataTable GetdAssAdminMemberDetail(int assIdx, int memberIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetdAssAdminMemberDetail(assIdx, memberIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 협회관리자등록을위한검색
        public DataSet GetAllAdminMember(int assIdx, int option, string search)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAllAdminMember(assIdx,option, search);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #endregion

        #region 협회회원관리

        #region 회원급수 비교
        public DataTable CheckMemberClassCompare(int assType, int editMember, DataTable checkList)
        {

            DataTable dt = null;
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB, DbTrans))
                {
                    BeginTransaction();
                    dt = da.CheckMemberClassCompare(assType, editMember, checkList);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                
            }

            return dt;
        }
        #endregion

        #region 엑셀 회원추가
        public DataTable AddMemberToExcel(int assType, DataTable memberList, int editMember)
        {

            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB, DbTrans))
                {
                    BeginTransaction();
                    dt = da.AddMemberToExcel(assType, memberList, editMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();

            }

            return dt;
        }
        #endregion

        #region 엑셀 클럽추가
        public DataTable AddClubToExcel(int assIdx, DataTable clubList, int editMember)
        {

            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB, DbTrans))
                {
                    BeginTransaction();
                    dt = da.AddClubToExcel(assIdx, clubList, editMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();

            }

            return dt;
        }
        #endregion

        #region 협회회원전체조회
        public DataSet GetAssAllMember(int assIdx, int region, int area, int option, int option2, string search, int start, int end)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAssAllMember(assIdx, region, area, option, option2, search, start, end);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 협회회원상세조회
        public DataSet GetAssAllMemberDetail(int clubIdx, int assIdx, int memberIdx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAssAllMemberDetail(clubIdx, assIdx, memberIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 협회회원수정
        public int EditAssAllMemberInfo(int memberIdx, int clubIdx, string birth, string phone, string gender,
                                        int assIdx, int regionClassCode, int areaClassCode, string reason,
                                        int dressCode, int shoesCode, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditAssAllMemberInfo(memberIdx, clubIdx, birth, phone, gender, assIdx, regionClassCode, areaClassCode, reason, dressCode, shoesCode, editMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 협회회원 급수 인증
        public int SetMemberClassAuth(int assIdx, DataTable classList, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB, DbTrans))
                {
                    BeginTransaction();
                    result = da.SetMemberClassAuth(assIdx, classList, editMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;

            }

            return result;
        }
        #endregion

        #region 협회회원 개별 등록
        public int AddMemberSeperate(int assIdx, int regionCode, DataTable memberList, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB, DbTrans))
                {
                    BeginTransaction();
                    result = da.AddMemberSeperate(assIdx, regionCode, memberList, editMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;

            }

            return result;
        }
        #endregion


        #endregion

        #region 직위, 등급

        #region 등급조회
        public DataTable GetAssGrade(int assIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetAssGrade(assIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }

        #endregion

        #region 직위조회
        public DataTable GetAssPosition(int assIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetAssPosition(assIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }

        #endregion

        #region 직위추가
        public int SetAssPosition(int assIdx, DataTable positions)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetAssPosition(assIdx, positions);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 직위수정
        public int EditAssPosition(int assIdx, int poCode, string position,int assManage, int noticeManage, int clubManage, int memberManage, int transferManage)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditAssPosition(assIdx, poCode, position, assManage, noticeManage, clubManage, memberManage, transferManage);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 직위삭제
        public int DelAssPosition(int assIdx, int poCode)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.DelAssPosition(assIdx, poCode);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion
        #endregion

        #region 가입대기

        #region 대기목록 조회
        public DataTable GetWaitClubList(int assIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetWaitClubList(assIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 가입 승인/거절/취소 처리
        public int SetAssClubWait(int assIdx, int clubIdx, int isOk, int editMember, string reason)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetAssClubWait(assIdx, clubIdx, isOk, editMember, reason);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 클럽수정

        #region 클럽명 변경 목록
        public DataSet GetClubEditList(int assIdx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetClubEditList(assIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 수정클럽 상세정보
        public DataTable GetClubEditDetail(int editIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetClubEditDetail(editIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 클럽명 변경 처리
        public int ClubNameEdit(int editIdx, int isOk, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.ClubNameEdit(editIdx, isOk, editMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 클럽명 변경 신청
        public int ReqClubNameChange(int clubIdx, string befClubName, string aftClubName, int reqMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.ReqClubNameChange(clubIdx, befClubName, aftClubName, reqMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 이적

        #region 이적리스트
        public DataSet GetAssTransferList(int assIdx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAssTransferList(assIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 이적 취소 처리
        public int CancelTransfer(int transIdx, string cancelReason, int editMember, int assIdx)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.CancelTransfer(transIdx, cancelReason, editMember, assIdx);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 이적 상세조회
        public DataTable GetAssTransferDetail(int transIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetAssTransferDetail(transIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #endregion

        #endregion

        #region 관리

        #region 관리목록
        public DataSet GetManageList(int memberIdx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetManageList(memberIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 관리상세조회
        public DataSet GetManageDetail(int memberIdx, string type, int idx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetManageDetail(memberIdx, type, idx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 클럽정보
        public DataSet GetClubInfo(int clubIdx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetClubInfo(clubIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #endregion

        #region 이적

        #region 이적신청
        public int ReqTransfer(int memberIdx, int befRegionCode, int befAreaCode, int clubIdx,
                               int aftRegionCode, int aftAreaCode, int tClubIdx, string filePath, string reason, int reqMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.ReqTransfer(memberIdx, befRegionCode, befAreaCode, clubIdx, aftRegionCode, aftAreaCode, tClubIdx, filePath, reason, reqMember);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 이적리스트
        public DataTable GetTransferList(int clubIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetTransferList(clubIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 이적 승인/거절
        public int EditTransfer(int transIdx, int editMember, int isOk, string editReason)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditTransfer(transIdx, editMember, isOk, editReason);
                    CommitTransaction();
                }

            }
            catch (Exception exp)
            {
                RollBackTransaction();
                return -99;
            }

            return result;
        }
        #endregion

        #region 이적상세조회
        public DataSet GetTransferDetail(int transIdx)
        {
            DataSet dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetTransferDetail(transIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        
        #endregion

        #endregion

        #region 클럽

        #region 클럽리스트
        public DataTable GetClubList(int clubIdx, int regionCode, int areaCode)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetClubList(clubIdx, regionCode, areaCode);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 클럽정보
        public DataSet GetClub(int clubIdx)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetClub(clubIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 클럽수정
        public int EditClub(int clubIdx, int memberIdx, string clubName, int stadiumIdx, string memo)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditClub(clubIdx, memberIdx, clubName, stadiumIdx, memo);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 클럽 회원 리스트
        public DataSet GetClubMemberList(int clubIdx)
        {
            DataSet ds = null;

            try
            {
                using(var da = new Api_DA(DB))
                {
                    ds = da.GetClubMemberList(clubIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 클럽 회원 정보
        public DataSet GetClubMemberInfo(int memberIdx, int clubIdx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetClubMemberInfo(memberIdx, clubIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region (클럽)회원수정
        public int EditMemberInfo(int memberIdx, int clubIdx, string birth, string phone, string gender, int gradeCode, int regionClassCode, int areaClassCode,
                                                string addr1, string addr2, int dressCode, int shoesCode)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditMemberInfo(memberIdx, clubIdx, birth, phone, gender, gradeCode, regionClassCode, areaClassCode,
                                                addr1, addr2, dressCode, shoesCode);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 가입 대기 리스트 
        public DataSet GetClubMemberWaitList(int clubIdx)
        {
            DataSet ds = null;

            try
            {
                using(var da = new Api_DA(DB))
                {
                    ds = da.GetClubMemberWaitList(clubIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 가입 승인
        public int ConfirmWaitClubMember(int waitIdx, int optionCode, int editIdx, bool isAccept)
        {
            int rtn = 0;

            try
            {
                BeginTransaction();
                using(var da = new Api_DA(DB, DbTrans))
                {
                    rtn = da.ConfirmWaitClubMember(waitIdx, optionCode, editIdx, isAccept);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                throw exp;
            }

            return rtn;
        }
        #endregion

        #region 가입 거절
        public int RejectWaitClubMember(int waitIdx, string rejectReason, int editIdx, bool isAccept)
        {
            int rtn = 0;

            try
            {
                BeginTransaction();
                using(var da = new Api_DA(DB, DbTrans))
                {
                    rtn = da.RejectWaitClubMember(waitIdx, rejectReason, editIdx, isAccept);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return rtn;
        }
        #endregion

        #region 클럽 회원 추가/수정
        public int SetClubMember(DataTable members, int editIdx
            )
        {
            int rtn = 0;

            try
            {
                BeginTransaction();
                using(var da = new Api_DA(DB, DbTrans))
                {
                    rtn = da.SetClubMember(members, editIdx);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                throw exp;
            }

            return rtn;
        }
        #endregion

        #region 클럽 회원(복수) 급수 변경
        public int SetClubMembersClasses(DataTable regionClassCode, DataTable areaClassCode, int editIdx)
        {
            int rtn = 0;

            try
            {
                BeginTransaction();
                using (var da = new Api_DA(DB, DbTrans))
                {
                    rtn = da.SetClubMembersClasses(regionClassCode, areaClassCode, editIdx);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                throw exp;
            }

            return rtn;
        }
        #endregion

        #region 클럽 회원 급수 조회
        public DataSet GetClubMembersClasses(int assIdx, int region, int area, int option, string search)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetClubMembersClasses(assIdx, region,area, option, search);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 클럽 회원 급수 수정
        public int EditClubMembersClasses(int assIdx, int assType, string reason, DataTable classList, int editMember)
        {
            int rtn = 0;
            try
            {
                BeginTransaction();
                using (var da = new Api_DA(DB, DbTrans))
                {
                    rtn = da.EditClubMembersClasses(assIdx, assType, reason, classList, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                throw exp;
            }

            return rtn;
        }
        #endregion

        #region 제명

        #region 제명 조회
        public DataTable GetExpulsion(string type, int idx, int memberIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetExpulsion(type, idx, memberIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion  

        #region 제명변경
        public int EditExpulsion(int expIdx, DateTime srtDate, DateTime endDate, Boolean isExp, string expReason, int editMember)
        {
            int rtn = 0;

            try
            {
                BeginTransaction();
                using(var da = new Api_DA(DB, DbTrans))
                {
                    rtn = da.EditExpulsion(expIdx, srtDate, endDate, isExp, expReason, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                throw exp;
            }

            return rtn;
        }
        #endregion

        #region 제명취소
        public int CancelExpulsion(int expIdx)
        {
            int rtn = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    rtn = da.CancelExpulsion(expIdx);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
            }

            return rtn;
        }
        #endregion

        #region 제명등록
        public int SetExpulsion(string type, int idx, int memberIdx, DateTime startDate, DateTime endDate, string expReason, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetExpulsion(type, idx, memberIdx, startDate, endDate, expReason, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #endregion

        #endregion

        #region 공통

        #region 협회명 반환
        public DataTable GetAssName(int assIdx)
        {
            DataTable dt = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetAssName(assIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;

        }
        #endregion

        #region 임원진 정보 모달
        public DataTable GetAdminInfoToModal(int memberIdx)
        {
            DataTable dt = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetAdminInfoToModal(memberIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 엑셀

        #region 급수비교양식다운로드
        public DataTable GetClassExcelForm(string type, int idx)
        {
            DataTable ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetClassExcelForm(type, idx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #endregion

        #region 클럽 등록시 검색
        public DataSet GetAssClubSearch(int regionCode, int option, int areaCode, string search)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAssClubSearch(regionCode, option, areaCode, search);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 전체회원 검색
        public DataSet GetAllMember(int assIdx, int options, string search)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAllMember(assIdx, options, search);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;

        }
        #endregion

        #region 지역
        public DataTable GetRegion()
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetRegion();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 시도
        public DataTable GetArea(int regionCode)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetArea(regionCode);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 클럽명 중복확인
        public int ClubNameOverlap(int region, int area, string clubName)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    result = da.ClubNameOverlap(region, area, clubName);
                }
            }
            catch (Exception exp)
            {

                result = -99;
            }

            return result;
        }
        #endregion

        #region 협회명 중복확인
        public int AssNameOverlap(int region, int area, string assName)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    result = da.AssNameOverlap(region, area, assName);
                }
            }
            catch (Exception exp)
            {

                result = -99;
            }

            return result;
        }
        #endregion

        #region 소속클럽리스트 조회
        public DataTable GetBelongClubList(int region, int area, int assIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetBelongClubList(region, area, assIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 미가입회원 동명이인
        public int GetDuplicateUnsingMember(int clubIdx, string memberName, string gender, string birth)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    result = da.GetDuplicateUnsingMember(clubIdx, memberName, gender, birth);
                }
            }
            catch (Exception exp)
            {

                result = -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 업로드

        #region 사진
        public int UploadImage(string type, int idx, DataTable images)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.UploadImage(type, idx, images);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 파일
        public int UploadFile(string type, int idx, DataTable files)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.UploadFile(type, idx, files);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 폴더명 가져오기
        public DataTable GetFolderName(string addType,string type, int idx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetFolderName(addType,type, idx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }

        #endregion

        #region 댓글 폴더명 가져오기
        public DataTable GetReplyFolderName(int repIdx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetReplyFolderName(repIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }

        #endregion

        #region 사진 수정/삭제
        public int EditUpload(string type, int idx, DataTable files)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditUpload(type, idx, files);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion
        
        #region 파일 수정/삭제
        public int EditFiles(string type, int idx, DataTable files)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditFiles(type, idx, files);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion



        #endregion

        #region 파일

        #region 파일목록
        public DataTable GetFileList(string type, int idx)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetFileList(type, idx);
                }
            }
            catch (Exception e)
            {

                throw e;
            }

            return dt;
        }
        #endregion

        #endregion

        #region 공지사항

        #region 공지사항 삭제
        public int DeleteNotice(int noticeIdx, int memberIdx)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.DeleteNotice(noticeIdx, memberIdx);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 공지사항 수정
        public int EditNotice(int noticeIdx, int memberIdx, string title, string contents, int isRegion, int isArea, int isClub)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditNotice(noticeIdx, memberIdx, title, contents, isRegion, isArea, isClub);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 공지사항 등록
        public int SetNotice(int memberIdx, int assIdx, string title, string contents, int isRegion, int isArea, int isClub)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetNotice(memberIdx, assIdx, title, contents, isRegion, isArea, isClub);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 공지사항 조회 리스트
        public DataSet GetNoticeList(string division, int assIdx, string type, string search, int start, int end)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetNoticeList(division, assIdx, type, search, start, end);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 공지사항 상세 조회
        public DataSet GetNoticeDetail(int noticeIdx)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetNoticeDetail(noticeIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #endregion

        #region 댓글 

        #region 댓글 수정
        public int EditReply(int memberIdx, int noticeIdx, int repIdx, string contents, string filePath)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditReply(memberIdx, noticeIdx, repIdx, contents, filePath);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 댓글 작성
        public int SetReply(int memberIdx, int noticeIdx, int repIdx, int depth, string contents, string filePath)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetReply(memberIdx, noticeIdx, repIdx, depth, contents, filePath);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 댓글 조회
        public DataSet GetReplyList(int noticeIdx)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetReplyList(noticeIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;

        }
        #endregion

        #region 댓글 삭제
        public int DelReply(int repIdx, int memberIdx)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.DelReply(repIdx, memberIdx);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 경기장

        #region 경기장 조회
        public DataTable GetStadium(int regionCode, int areaCode, string search)
        {
            DataTable dt = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    dt = da.GetStadium(regionCode, areaCode, search);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return dt;
        }
        #endregion

        #region 경기장 요청
        public int SetStadiumRquest(int memberIdx, int regionCode, int areaCode, string stadiumName, int clubIdx)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetStadiumRquest(memberIdx, regionCode, areaCode, stadiumName, clubIdx);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 어드민

        #region 관리정보
        public DataSet GetAdminManageDetail()
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAdminManageDetail();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 회원

        #region 회원목록
        public DataSet GetStatncoAdminMemberList(int block, int option, string search, int start , int end)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminMemberList(block, option, search, start, end);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 회원상세
        public DataSet GetStatncoAdminMemberDetail(int regAssIdx, int areAssIdx, int memberIdx)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminMemberDetail( regAssIdx,  areAssIdx,  memberIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 회원탈퇴
        public int DelMember(int memberIdx, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.DelMember(memberIdx, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 차단

        #region 회원차단
        public int EditMemberblock(int memberIdx, int type, string memo, string start, string end, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditMemberblock(memberIdx, type, memo,start, end, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 차단변경
        public int editBlock(int memberIdx, int type, string memo, string start, string end, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.editBlock(memberIdx, type, memo, start, end, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 클럽

        #region 클럽목록
        public DataSet GetStatncoAdminClubList(int region, int area, int option, string search, int start, int end)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminClubList(region, area, option, search, start, end);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 클럽정보
        public DataSet GetAdminClubInfo(int clubIdx)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAdminClubInfo(clubIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 클럽회원전체조회
        public DataSet GetAllClubMember(int clubIdx, int option, string search)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetAllClubMember(clubIdx, option, search);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 클럽메모작성
        public int RegistClubMemo(int clubIdx, string memo, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.RegistClubMemo(clubIdx, memo, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 클럽폐쇄
        public int DeleteClub(int clubIdx, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.DeleteClub(clubIdx, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 클럽장양도
        public int AssignClubMaster(int clubIdx, int memberIdx, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.AssignClubMaster(clubIdx, memberIdx, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 협회

        #region 협회상세조회
        public DataSet GetStatncoAdminAssociationDetail(int assIdx)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminAssociationDetail(assIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 협회생성
        public int CreateAssociation(int region , int area, string assName, string assType, int memberIdx, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.CreateAssociation(region, area, assName, assType, memberIdx, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 협회목록
        public DataSet GetStatncoAdminAssociationList(int region, int area, int option, string search, int start, int end)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminAssociationList(region, area, option, search, start, end);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 협회메모작성
        public int RegistAssMemo(int assIdx, string memo, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.RegistAssMemo(assIdx, memo, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 협회셋팅조회
        public DataTable GetStatncoAdminAssociationDetailSetting(int assIdx)
        {
            DataTable ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminAssociationDetailSetting(assIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 협회셋팅변경
        public int EditStatncoAssocationDetailSetting(int assIdx, int regionCode, int areaCode, string assName, string assType, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditStatncoAssocationDetailSetting(assIdx, regionCode, areaCode, assName, assType, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 협회마스터양도
        public int AssingAssMaster(int assIdx, int memberIdx, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.AssingAssMaster(assIdx, memberIdx, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {
                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 체육관

        #region 체육관 목록
        public DataSet GetStatncoAdminStadiumList(int option, string search, int start, int end)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminStadiumList(option, search, start, end);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 체육관 상세정보
        public DataSet GetStatncoAdminStadiumDetail(int stadiumIdx)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminStadiumDetail(stadiumIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 체육관 메모 수정
        public int EditStadiumMemo(int stadiumIdx, string memo, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditStadiumMemo(stadiumIdx,  memo,  editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 체육관 검색어 목록 수정
        public int EditStadiumSearchList(int stadiumIdx, DataTable searchList)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditStadiumSearchList(stadiumIdx, searchList);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 체육관 정보 수정
        public int EditStadiumDetail(int stadiumIdx, int regionCode, int areaCode, int type, int status,
                                      string stadiumName, string stadiumAddr, string stadiumAddr2, string phone, string searchList, int isUse, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.EditStadiumDetail( stadiumIdx,  regionCode,  areaCode,  type,  status,
                                                   stadiumName,  stadiumAddr,  stadiumAddr2,  phone, searchList, isUse, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 체육관 요청 목록
        public DataSet GetStatncoAdminStadiumRequestList(int option, string search, int start, int end)
        {
            DataSet ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminStadiumRequestList(option, search, start, end);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 체육관 등록
        public int CreateStadium( int regionCode, int areaCode, int type, int status,
                                  string stadiumName, string stadiumAddr, string stadiumAddr2, string phone, string searchList, int isUse, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.CreateStadium( regionCode, areaCode, type, status,
                                                   stadiumName, stadiumAddr, stadiumAddr2, phone, searchList, isUse, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #region 체육관 요청 상세정보
        public DataTable GetStatncoAdminStadiumRequestDetail(int requestIdx)
        {
            DataTable ds = null;
            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminStadiumRequestDetail(requestIdx);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #region 체육관 요청처리
        public int SetStatncoAdminStadiumRequestEdit(int requestIdx, int regionCode, int areaCode, int type,
                                                     int status, string stadiumName, string stadiumAddr, string stadiumAddr2, string phone,
                                                     string tag, int isUse, int editMember)
        {
            int result = 0;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    BeginTransaction();
                    result = da.SetStatncoAdminStadiumRequestEdit(requestIdx, regionCode, areaCode, type, status, stadiumName, 
                                                                  stadiumAddr, stadiumAddr2, phone, tag, isUse, editMember);
                    CommitTransaction();
                }
            }
            catch (Exception exp)
            {

                RollBackTransaction();
                result = -99;
            }

            return result;
        }
        #endregion

        #endregion

        #region 커뮤니티

        #region 게시글 조회
        public DataSet GetStatncoAdminBoardList(string boardType, int option, string search, int start, int end)
        {
            DataSet ds = null;

            try
            {
                using (var da = new Api_DA(DB))
                {
                    ds = da.GetStatncoAdminBoardList(boardType, option, search,start, end);
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return ds;
        }
        #endregion

        #endregion

        #endregion

       
    }
}


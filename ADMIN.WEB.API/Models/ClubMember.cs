using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADMIN.WEB.API.Models
{
    public class ClubMembers
    {
        public MmeberInfo[] members { get; set; }

        public class MmeberInfo
        {
            public int memberIdx { get; set; }
            public int clubIdx { get; set; }
            public string memberId { get; set; }
            public string memberName { get; set; }
            public string gender { get; set; }
            public string birth { get; set; }
            public int gradeCode { get; set; }
            public string phone { get; set; }
            public int regionClassCode { get; set; }
            public int areaClassCode { get; set; }
            public string addr1 { get; set; }
            public string addr2 { get; set; }
            public int dressCode { get; set; }
            public int shoesCode { get; set; }
        }

        public class EditMember 
        {
            public int memberIdx { get; set; }
            public int clubIdx { get; set; }
            public string memberId { get; set; }
            public string memberName { get; set; }
            public string gender { get; set; }
            public string birth { get; set; }
            public int gradeCode { get; set; }
            public string phone { get; set; }
            public int regionClassCode { get; set; }
            public int areaClassCode { get; set; }
            public string addr1 { get; set; }
            public string addr2 { get; set; }
            public int dressCode { get; set; }
            public int shoesCode { get; set; }
        }


        public class ClubMember
        {
            public int clubIdx { get; set; }
            public int memberIdx { get; set; }
        }
    }
}
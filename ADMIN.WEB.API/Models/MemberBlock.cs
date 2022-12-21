namespace ADMIN.WEB.API.Models
{
    public class MemberBlock
    {
        public int memberIdx { get; set; }
        public int type { get; set; }
        public string memo { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}
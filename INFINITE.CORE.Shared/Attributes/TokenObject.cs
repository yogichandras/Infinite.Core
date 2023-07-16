namespace INFINITE.CORE.Shared.Attributes
{
    public class TokenObject
    {
        public TokenUserObject User { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string RawToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class TokenUserObject
    {
        public Guid Id { get; set; }
        public List<ReferensiStringObject> Role { get; set; }
        public List<string> Permissions { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Mail { get; set; }
    }
}

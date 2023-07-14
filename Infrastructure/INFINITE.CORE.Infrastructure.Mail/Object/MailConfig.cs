
namespace INFINITE.CORE.Infrastructure.Mail.Object
{
    public class MailConfig
    {
        public string Smtp { get; set; }
        public int SmtpPort { get; set; }
        public string SenderMail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AttachmentMail
    {
        public Stream File { get; set; }
        public string Name { get; set; }
    }
}
using System.Net;
using System.Net.Mail;
using INFINITE.CORE.Infrastructure.Mail.Interface;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using INFINITE.CORE.Infrastructure.Mail.Object;
using Microsoft.Extensions.Options;

namespace INFINITE.CORE.Infrastructure.Mail.Service
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly MailConfig _config;
        private string WebUrl = "";
        public EmailService(ILogger<EmailService> logger,
            IOptions<MailConfig> config,
            IConfiguration _configuration)
        {
            _logger = logger;
            _config = config.Value;
            WebUrl = _configuration["WebURL"];
        }

        #region SendMail
        public async Task<(bool Success, string Message, Exception ex)> SendMail(List<string> to, List<string> cc, string subject, string body, List<AttachmentMail> attachments)
        {
            try
            {

                if (to == null || to.Count == 0)
                    return (false, "To Mail cannot be empty or null!", null);

                using (var smtpClient = new SmtpClient(_config.Smtp, _config.SmtpPort))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential()
                    {
                        UserName = _config.UserName,
                        Password = _config.Password,
                    };
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(_config.UserName, _config.SenderMail);
                    foreach (var m in to)
                    {
                        if (!IsValidEmail(m))
                            return (false, "Email tidak valid", null);

                        mail.To.Add(m);
                    }

                    if (cc != null)
                    {
                        if (cc.Count > 0)
                        {
                            foreach (var c in cc)
                            {
                                if (!IsValidEmail(c))
                                    return (false, "Email tidak valid", null);

                                mail.CC.Add(c);
                            }
                        }
                    }

                    if (attachments != null)
                    {
                        if (attachments.Count > 0)
                        {
                            foreach (var d in attachments)
                            {
                                mail.Attachments.Add(new Attachment(d.File, d.Name));
                            }
                        }
                    }

                    mail.IsBodyHtml = true;
                    mail.Subject = subject;
                    mail.Body = body;
                    await smtpClient.SendMailAsync(mail);
                }
                return (true, "OK", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed Send Email", new { subject = subject, message = body });
                return (false, ex.Message, ex);
            }
        }
        #endregion

        #region IsValid Email
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        #endregion

    }
}
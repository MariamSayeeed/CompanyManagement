using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MVC03.PL.Settings;

namespace MVC03.PL.Helpers
{
    public class MailService : IMailService
    {
        private readonly MailSettings _options;

        public MailService(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }
        public bool SendEmail(Email email)
        {
            // build message
            var mail = new MimeMessage();

            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress(_options.DisplayName ,_options.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();

            builder.TextBody =email.Body;
            mail.Body = builder.ToMessageBody();

            // Establish connection 

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host,int.Parse( _options.Port) , MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);


            // send message

            smtp.Send(mail);
            return true;

        }
    }
}

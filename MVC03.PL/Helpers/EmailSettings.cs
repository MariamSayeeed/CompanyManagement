using System.Net;
using System.Net.Mail;

namespace MVC03.PL.Helpers
{
    public class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            try
            {

                // mail server -> Gmail
                // protocol -> for data transfer :*SMTP* : Simple Mail Transfer Protocol 

                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;

                client.Credentials = new NetworkCredential("ms1023993979@gmail.com", "vimhkrsjmujkwvlb");  // Sender Data
                client.Send("ms1023993979@gmail.com", email.To, email.Subject, email.Body);

                return true;

            }
            catch (Exception e) 
            {
                return false;
            }


        }
    }
}

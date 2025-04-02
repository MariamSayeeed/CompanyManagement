using Twilio.Rest.Api.V2010.Account;

namespace MVC03.PL.Helpers
{
    public interface ITwilioService

    {
        public MessageResource SendSMS(SMS sms);
    }
}

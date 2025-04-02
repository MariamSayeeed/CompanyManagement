using Microsoft.Extensions.Options;
using MVC03.PL.Settings;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MVC03.PL.Helpers
{
    public class TwilioService : ITwilioService
    {
        private readonly TwilioSettings _options;

        public TwilioService(IOptions<TwilioSettings> options)
        {
            _options = options.Value;
        }
        public MessageResource SendSMS(SMS sms)
        {
            // Establish Connection 
            TwilioClient.Init(_options.AccountSID , _options.AuthToken);

            // build Message
            var message = MessageResource.Create
                (
                body: sms.Body,
                to: sms.To,
                from: new Twilio.Types.PhoneNumber( _options.PhoneNumber)

                );
            
            // Send Message
        
            return message;
        
        }
    }
}

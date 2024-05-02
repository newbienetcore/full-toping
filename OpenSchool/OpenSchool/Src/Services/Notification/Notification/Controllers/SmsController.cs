using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Notification.Controllers;

public class SmsController : BaseController
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromPhoneNumber;

    public SmsController()
    {
        _accountSid = "ACf90c171987771aa61901a2cafd9cb50a";
        _authToken = "e022e0c06db0fe6875667ee42ee8bd91";
        _fromPhoneNumber = "+14695072668";
    }
    
    [HttpPost]
    public IActionResult SendSms(string to, string message)
    {
        try
        {
            TwilioClient.Init(_accountSid, _authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber(to))
            {
                From = new PhoneNumber(_fromPhoneNumber),
                Body = message
            };

            var twilioMessage = MessageResource.Create(messageOptions);

            return Ok($"SMS sent successfully. Message SID: {twilioMessage.Sid}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to send SMS: {ex.Message}");
        }
    }
}
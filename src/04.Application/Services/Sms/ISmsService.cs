using Pertamina.SIMIT.Application.Services.Sms.Models.SendSms;

namespace Pertamina.SIMIT.Application.Services.Sms;

public interface ISmsService
{
    Task SendSmsAsync(SendSmsRequest sendSmsRequest);
}

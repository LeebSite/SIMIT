using Pertamina.SIMIT.Application.Services.DateAndTime;

namespace Pertamina.SIMIT.Infrastructure.DateAndTime;

public class DateAndTimeService : IDateAndTimeService
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}

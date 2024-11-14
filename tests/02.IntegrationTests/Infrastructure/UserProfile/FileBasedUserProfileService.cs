using Microsoft.Extensions.Logging;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.UserProfile;
using Pertamina.SIMIT.Application.Services.UserProfile.Models.GetUserProfile;
using Pertamina.SIMIT.IntegrationTests.Repositories.Users;
using Pertamina.SIMIT.IntegrationTests.Repositories.Users.Models;

namespace Pertamina.SIMIT.IntegrationTests.Infrastructure.UserProfile;

public class FileBasedUserProfileService : IUserProfileService
{
    private readonly ILogger<FileBasedUserProfileService> _logger;

    public FileBasedUserProfileService(ILogger<FileBasedUserProfileService> logger)
    {
        _logger = logger;
    }

    public Task<GetUserProfileResponse> GetUserProfileAsync(string username, CancellationToken cancellationToken)
    {
        try
        {
            var user = UserRepository.Users.Where(x => x.Username == username).SingleOrDefault();

            if (user is null)
            {
                throw new NotFoundException(nameof(User), nameof(User.Username), username);
            }

            var result = new GetUserProfileResponse
            {
                Id = user.Id,
                Username = user.Username,
                DisplayName = user.DisplayName,
                EmailAddress = user.Username,
                EmployeeId = user.EmployeeId
            };

            return Task.FromResult(result);

        }

        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error in executing method {nameof(GetUserProfileAsync)}");

            throw;
        }
    }
}

using System.Reflection;
using MediatR;
using Pertamina.SIMIT.Application.Common.Attributes;
using Pertamina.SIMIT.Application.Common.Exceptions;
using Pertamina.SIMIT.Application.Services.Authorization;
using Pertamina.SIMIT.Application.Services.CurrentUser;
using Pertamina.SIMIT.Shared.Services.Authentication.Constants;
using Pertamina.SIMIT.Shared.Services.Authorization.Constants;

namespace Pertamina.SIMIT.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private const string StartingErrorMessage = "The server is refusing to process the request because the user";
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationBehaviour(ICurrentUserService currentUserService, IAuthorizationService authorizationService)
    {
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedAccessException($"{StartingErrorMessage} is not authenticated.");
            }

            var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));

            if (authorizeAttributesWithPolicies.Any())
            {
                if (string.IsNullOrWhiteSpace(_currentUserService.PositionId))
                {
                    throw new ForbiddenAccessException($"{StartingErrorMessage} {_currentUserService.Username} does not have {AuthenticationDisplayTextFor.PositionId}.");
                }

                var authorizationInfo = await _authorizationService.GetAuthorizationInfoAsync(_currentUserService.PositionId, cancellationToken);

                foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                {
                    var authorized = authorizationInfo.Roles.SelectMany(x => x.Permissions).Any(x => x.Equals(policy, StringComparison.OrdinalIgnoreCase));

                    if (!authorized)
                    {
                        throw new ForbiddenAccessException($"{StartingErrorMessage} {_currentUserService.Username} with {AuthenticationDisplayTextFor.PositionId} {_currentUserService.PositionId} does not have the following {AuthorizationClaimTypes.Permission}: {policy}");
                    }
                }
            }
        }

        return await next();
    }
}

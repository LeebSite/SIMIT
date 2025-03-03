﻿using MediatR;
using Microsoft.Extensions.Logging;
using Pertamina.SIMIT.Application.Common.Extensions;
using Pertamina.SIMIT.Application.Services.CurrentUser;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUser;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            var requestName = typeof(TRequest).Name;
            var formattedRequest = request.ToPrettyJson();
            var username = _currentUser.Username;
            var ipAddress = _currentUser.IpAddress;
            var latitude = _currentUser.Geolocation is null ? DefaultTextFor.NA : _currentUser.Geolocation.Latitude.ToString();
            var longitude = _currentUser.Geolocation is null ? DefaultTextFor.NA : _currentUser.Geolocation.Longitude.ToString();
            var accuracy = _currentUser.Geolocation is null ? DefaultTextFor.NA : _currentUser.Geolocation.Accuracy.ToString();

            _logger.LogError(exception, "Unhandled Exception when executing {RequestName} by {Username} from {IpAddress} at {Latitude}||{Longitude}||{Accuracy}.\n{RequestName}\n{RequestObject}",
               requestName, username, ipAddress, latitude, longitude, accuracy, requestName, formattedRequest);

            throw;
        }
    }
}

using FastEndpoints;
using JetBrains.Annotations;

namespace Hana.Abstractions;


public abstract class HanaEndpointWithMapper<TRequest, TMapper> : EndpointWithMapper<TRequest, TMapper> where TMapper : class, IRequestMapper where TRequest : notnull
{
    protected void ConfigurePermissions(params string[] permissions)
    {
        if (!EndpointSecurityOptions.SecurityIsEnabled)
            AllowAnonymous();
        else
            Permissions([PermissionNames.All, .. permissions]);
    }
}

public abstract class HanaEndpointWithoutRequest : EndpointWithoutRequest
{
    protected void ConfigurePermissions(params string[] permissions)
    {
        if (!EndpointSecurityOptions.SecurityIsEnabled)
            AllowAnonymous();
        else
            Permissions([PermissionNames.All, .. permissions]);
    }
}

public abstract class HanaEndpointWithoutRequest<TResponse> : EndpointWithoutRequest<TResponse> where TResponse : notnull
{
    protected void ConfigurePermissions(params string[] permissions)
    {
        if (!EndpointSecurityOptions.SecurityIsEnabled)
            AllowAnonymous();
        else
            Permissions([PermissionNames.All, .. permissions]);
    }
}

public class HanaEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse> where TRequest : notnull, new() where TResponse : notnull
{
    protected void ConfigurePermissions(params string[] permissions)
    {
        if (!EndpointSecurityOptions.SecurityIsEnabled)
            AllowAnonymous();
        else
            Permissions([PermissionNames.All, .. permissions]);
    }
}

public class HanaEndpoint<TRequest, TResponse, TMapper> : Endpoint<TRequest, TResponse, TMapper> where TRequest : notnull, new() where TResponse : notnull where TMapper : class, IMapper, new()
{
    protected void ConfigurePermissions(params string[] permissions)
    {
        if (!EndpointSecurityOptions.SecurityIsEnabled)
            AllowAnonymous();
        else
            Permissions([PermissionNames.All, .. permissions]);
    }
}

public class HanaEndpoint<TRequest> : Endpoint<TRequest> where TRequest : notnull, new()
{
    protected void ConfigurePermissions(params string[] permissions)
    {
        if (!EndpointSecurityOptions.SecurityIsEnabled)
            AllowAnonymous();
        else
            Permissions([PermissionNames.All, .. permissions]);
    }
}


using Hana.Abstractions;
using Hana.Api.Common.Models;
using Hana.Users.Contract;
using Hana.Users.Entities;
using Hana.Users.Models;
using Hana.Users.Services;
using JetBrains.Annotations;

namespace Hana.Users.Endpoints;

[PublicAPI]
internal class Register(IUserServices services) : HanaEndpoint<CreateUserRequest>
{
    private readonly IUserServices _services = services;

    public override void Configure()
    {
        Post("/users/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        await _services.Create(request);
    }
}

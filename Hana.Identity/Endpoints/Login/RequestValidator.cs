using FastEndpoints;
using FluentValidation;
using JetBrains.Annotations;

namespace Hana.Identity.Endpoints.Login;

[PublicAPI]
internal class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Username).NotNull();
        RuleFor(x => x.Password).NotNull();
    }
}
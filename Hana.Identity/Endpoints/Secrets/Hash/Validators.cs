using FastEndpoints;
using FluentValidation;
using JetBrains.Annotations;

namespace Hana.Identity.Endpoints.Secrets.Hash;

[PublicAPI]
internal class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Secret).NotEmpty();
    }    
}
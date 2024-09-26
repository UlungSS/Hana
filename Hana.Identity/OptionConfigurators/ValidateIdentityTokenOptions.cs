using Hana.Common.Exceptions;
using Hana.Identity.Options;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Hana.Extensions;

public class ValidateIdentityTokenOptions : IPostConfigureOptions<IdentityTokenOptions>
{
    public void PostConfigure(string? name, IdentityTokenOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.SigningKey))
            throw new MissingConfigurationException("SigningKey is required");
    }
}
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Hana.Core.Api.Requirements;

public record NotReadOnlyResource(string? WorkflowDefinition = default);


public record NotReadOnlyRequirement() : IAuthorizationRequirement;


/// <inheritdoc />
[PublicAPI]
public class NotReadOnlyRequirementHandler : AuthorizationHandler<NotReadOnlyRequirement, NotReadOnlyResource>
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NotReadOnlyRequirement requirement, NotReadOnlyResource resource)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        //if (_managementOptions.Value.IsReadOnlyMode)
        //{
        //    context.Fail(new AuthorizationFailureReason(this, "Workflow edit is not allowed when the read-only mode is enabled."));
        //}

        //if (resource.WorkflowDefinition != null && (resource.WorkflowDefinition.IsReadonly || resource.WorkflowDefinition.IsSystem))
        //{
        //    context.Fail(new AuthorizationFailureReason(this, "Workflow edit is not allowed for a readonly or system workflow."));
        //}

        context.Succeed(requirement);
    }
}
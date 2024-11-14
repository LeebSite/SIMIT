using Microsoft.AspNetCore.Mvc;
using Pertamina.SIMIT.Shared.Common.Constants;

namespace Pertamina.SIMIT.WebApi.Common.Attributes.IgnoreApiDocumentation;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class IgnoreApiDocumentationAttribute : ApiExplorerSettingsAttribute
{
    public IgnoreApiDocumentationAttribute(string environments) : base()
    {
        if (environments is not null)
        {
            var ignoreApiDocumentationEnvironments = environments.Split(',').Select(x => x.Trim());

            var isIgnoreApiDocumentation = ignoreApiDocumentationEnvironments.Any(x => x.Contains(CommonValueFor.EnvironmentName));

            IgnoreApi = isIgnoreApiDocumentation;
        }
    }
}

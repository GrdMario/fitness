namespace Fitness.Presentation.Api.Internal.Mvc
{
    using Fitness.Presentation.Api.Controllers;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using System.Reflection;

    internal sealed class InternalControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            var isInternal = !typeInfo.IsAbstract && typeof(ApiControllerBase).IsAssignableFrom(typeInfo);
            return isInternal || base.IsController(typeInfo);
        }
    }
}
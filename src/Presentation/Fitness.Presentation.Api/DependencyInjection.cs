namespace Fitness.Presentation.Api
{
    using Fitness.Blocks.Common.Exceptions;
    using Fitness.Presentation.Api.Internal.Swagger;
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Linq;
    using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationConfiguration(this IServiceCollection services, IHostEnvironment environment)
        {
            void RouteOptions(RouteOptions options) => options.LowercaseUrls = true;

            void ProblemDetailsOptions(ProblemDetailsOptions options) => SetProblemDetailsOptions(options, environment);

            void NewtonsoftOptions(MvcNewtonsoftJsonOptions options)
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }

            services
                .AddRouting(RouteOptions)
                .AddProblemDetails(ProblemDetailsOptions)
                .AddControllers()
                .AddNewtonsoftJson(NewtonsoftOptions);

            services.AddSwaggerConfiguration();

            services.AddAuthentication("cookie")
                .AddCookie("cookie", o =>
                {
                    o.LoginPath = "authorization/login";
                });

            return services;
        }

        private static void SetProblemDetailsOptions(ProblemDetailsOptions options, IHostEnvironment env)
        {
            Type[] knownExceptionTypes = new Type[] { typeof(ServiceValidationException), typeof(ServiceAuthorizationException) };

            options.IncludeExceptionDetails = (_, exception) =>
                env.IsDevelopment() &&
                !knownExceptionTypes.Contains(exception.GetType());

            options.Map<ServiceValidationException>(exception =>
                new ValidationProblemDetails(exception.Errors)
                {
                    Title = exception.Title,
                    Detail = exception.Detail,
                    Status = StatusCodes.Status400BadRequest
                });

            options.Map<ServiceAuthorizationException>(exception =>
                new ProblemDetails()
                {
                    Title = exception.Title,
                    Detail = exception.Detail,
                    Status = StatusCodes.Status401Unauthorized
                });
        }
    }
}

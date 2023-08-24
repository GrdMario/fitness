namespace Fitness
{
    using Fitness.Application;
    using Fitness.Infrastructure.Adapter.Email;
    using Fitness.Infrastructure.Adapter.Security;
    using Fitness.Infrastructure.Db.Blob;
    using Fitness.Infrastructure.Db.Users;
    using Fitness.Presentation.Api;
    using Fitness.Presentation.Api.Internal.Swagger;
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;

    internal sealed class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public MssqlSettings MssqlSettings =>
            this.Configuration
            .GetSection(MssqlSettings.Key)
            .Get<MssqlSettings>() ?? throw new ArgumentNullException(nameof(this.MssqlSettings));

        public SecurityAdapterSettings SecurityAdapterSettings =>
           this.Configuration
           .GetSection(SecurityAdapterSettings.Key)
           .Get<SecurityAdapterSettings>() ?? throw new ArgumentNullException(nameof(this.SecurityAdapterSettings));

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddInfrastructureUsersConfiguration(this.MssqlSettings);
            services.AddApplicationLayer();
            services.AddSecurityAdapter(this.SecurityAdapterSettings);
            services.AddEmailAdapter();
            services.AddBlobLayer();
            services.AddPresentationConfiguration(this.Environment);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseProblemDetails();

            if (!this.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.MigrateMssqlDb();

            app.UseHttpsRedirection();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwaggerConfiguration();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

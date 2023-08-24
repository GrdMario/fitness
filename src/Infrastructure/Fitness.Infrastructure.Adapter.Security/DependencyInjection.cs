namespace Fitness.Infrastructure.Adapter.Security
{
    using Fitness.Application.Contracts.Security;
    using Fitness.Infrastructure.Adapter.Security.Internal;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using System.Threading.Tasks;

    public class SecurityAdapterSettingsFactory : IConfigureOptions<SecurityAdapterSettings>
    {
        private readonly IConfiguration _configuration;

        public SecurityAdapterSettingsFactory(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void Configure(SecurityAdapterSettings options)
        {
            this._configuration.GetSection(SecurityAdapterSettings.Key).Bind(options);
        }
    }

    public static class DependencyInjection
    {
        public static IServiceCollection AddSecurityAdapter(
            this IServiceCollection services,
            SecurityAdapterSettings settings)
        {
            services.ConfigureOptions<SecurityAdapterSettingsFactory>();

            services.AddScoped<IPasswordHashService, PasswordHashService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = settings.TokenSettings.Issuer,
                        ValidAudience = settings.TokenSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.TokenSettings.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true
                    };
                });

            return services;
        }
    }

    public class SecurityAdapterSettings
    {
        public const string Key = nameof(SecurityAdapterSettings);

        public TokenSettings TokenSettings { get; set; } = default!;

        public PasswordSettings PasswordSettings { get; set; } = default!;
    }

    public class TokenSettings
    {
        public string Audience { get; set; } = default!;

        public string Issuer { get; set; } = default!;

        public string Secret { get; set; } = default!;

        public int ExpiresInMinutes { get; set; } = default!;
    }

    public class PasswordSettings
    {
        /// <summary>
        ///     Minimum required length
        /// </summary>
        public int RequiredLength { get; set; } = 8;

        /// <summary>
        ///     Require a non letter or digit character
        /// </summary>
        public bool RequireNonLetterOrDigit { get; set; } = true;

        /// <summary>
        ///     Require a lower case letter ('a' - 'z')
        /// </summary>
        public bool RequireLowercase { get; set; } = true;

        /// <summary>
        ///     Require an upper case letter ('A' - 'Z')
        /// </summary>
        public bool RequireUppercase { get; set; } = true;

        /// <summary>
        ///     Require a digit ('0' - '9')
        /// </summary>
        public bool RequireDigit { get; set; } = true;
    }
}
namespace Fitness.Presentation.Api.Controllers.V1
{
    using Fitness.Application.Authentication;
    using Fitness.Application.Authentication.Models;
    using Fitness.Application.Contracts.Blob;
    using Fitness.Presentation.Api.Internal.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiVersion(ApiVersions.V1)]
    public class AuthenticationController : ApiControllerBase
    {
        public AuthenticationController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            return await this.ProcessAsync<LoginCommand, TokenResponse>(command);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [AllowAnonymous]
        [HttpGet("email-verification")]
        public async Task<IActionResult> EmailVerification([FromQuery] EmailVerificationCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [AllowAnonymous]
        [HttpPost("resend-email-verification")]
        public async Task<IActionResult> ResendEmailVerification([FromBody] ResendEmailVerificationCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            return await this.ProcessAsync<RefreshTokenCommand, TokenResponse>(command);
        }

        [Authorize]
        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            return await this.ProcessAsync(command);
        }

        [Authorize]
        [HttpGet("protected")]
        public async Task<IActionResult> Protected()
        {
            return Ok(await Task.FromResult("Protected route."));
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok(await Task.FromResult("Protected route."));
        }
    }
}

namespace Fitness.Application.Contracts.Security
{
    using Fitness.Domain;

    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        RefreshToken GenerateRefreshToken(User user);
    }
}

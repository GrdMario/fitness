namespace Fitness.Application.Contracts.Security
{
    public interface IPasswordHashService
    {
        string Hash(string password);

        bool VerifyHash(string guess, string password);
    }
}

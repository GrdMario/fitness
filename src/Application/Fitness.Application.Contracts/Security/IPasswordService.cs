namespace Fitness.Application.Contracts.Security
{
    using System.Collections.Generic;

    public interface IPasswordService
    {
        List<string> Validate(string password);

        string Generate();
    }
}

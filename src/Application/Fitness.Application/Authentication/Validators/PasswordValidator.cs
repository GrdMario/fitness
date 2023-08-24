namespace Fitness.Application.Authentication.Validators
{
    using Fitness.Application.Contracts.Security;
    using FluentValidation;
    using FluentValidation.Results;

    internal class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator(IPasswordService passwordService)
        {
            this.RuleFor(r => r)
                .Must((password, _, context) =>
                {
                    var errors = passwordService.Validate(password);

                    foreach (var error in errors)
                    {
                        context.AddFailure(new ValidationFailure("password", error));
                    }

                    return errors.Count == 0;
                });
        }
    }
}

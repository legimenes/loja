using FluentValidation;
using FluentValidation.Results;

namespace Loja.Core.Identity.OAuth
{
    public class TokenRequestValidator : AbstractValidator<TokenRequest>
    {
        public TokenRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(m => m.grant_type).Must(ValidateGrantType).WithMessage("Grant type is invalid");

            When(m => m.grant_type == "password", () =>
            {
                RuleFor(m => m.username)
                    .NotEmpty().WithMessage("Username is required")
                    .NotNull().WithMessage("Username is required");
                RuleFor(m => m.password)
                    .NotEmpty().WithMessage("Password is required")
                    .NotNull().WithMessage("Password is required");
            });

            When(m => m.grant_type == "refresh_token", () =>
            {
                RuleFor(m => m.refresh_token)
                    .NotEmpty().WithMessage("Refresh token is required")
                    .NotNull().WithMessage("Refresh token is required");
            });
        }

        private static bool ValidateGrantType(string grantType)
        {
            return grantType.Equals("authorization_code") || grantType.Equals("client_credentials")
                || grantType.Equals("password") || grantType.Equals("refresh_token");
        }

        protected override bool PreValidate(ValidationContext<TokenRequest> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure("", "Please ensure a model was supplied."));
                return false;
            }
            return true;
        }
    }
}
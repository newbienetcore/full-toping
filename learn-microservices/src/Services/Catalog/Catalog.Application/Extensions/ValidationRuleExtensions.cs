using System.Text.RegularExpressions;
using Catalog.Application.Properties;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.Localization;

namespace Catalog.Application;

public static class ValidationRuleExtensions
{
    public static IRuleBuilderOptionsConditions<T, string> NotPassword<T>(this IRuleBuilder<T, string> ruleBuilder, IStringLocalizer<Resources> localizer)
    {
        return ruleBuilder.Custom((password, context) =>
        {
            if (string.IsNullOrEmpty(password))
                context.AddFailure(localizer["the_password_is_required"].Value);
            else if (password.Length < 8 || password.Length > 30)
                context.AddFailure(localizer["the_password_must_be_8_30_characters_long"].Value);
            else if (!Regex.IsMatch(password, @"\d+", RegexOptions.Singleline))
                context.AddFailure(localizer["the_password_must_contains_at_least_1_numeric_[0_9]"].Value);
            else if (!Regex.IsMatch(password, @"[a-z]", RegexOptions.Singleline))
                context.AddFailure(localizer["the_password_must_contains_at_least_1_lowercase_character_[a_z]"].Value);
            else if (!Regex.IsMatch(password, @"[A-Z]", RegexOptions.Singleline))
                context.AddFailure(localizer["the_password_must_contains_at_least_1_uppercase_character_[A_Z]"].Value);
            else if (!Regex.IsMatch(password, @"[\*\.\!\@\#\$\%\^\&\(\)\{\}\[\]\:\;\<\>\,\.\?\/\~_\+\-\=\|\\]", RegexOptions.Singleline))
                context.AddFailure(localizer["the_password_must_contains_at_least_1_special_character"].Value);
        });
    }
    
    public static IRuleBuilderOptionsConditions<T, string> NotPhone<T>(this IRuleBuilder<T, string> ruleBuilder, IStringLocalizer<Resources> localizer)
    {
        return ruleBuilder.Custom((phone, context) =>
        {
            if (string.IsNullOrEmpty(phone))
                context.AddFailure(localizer["the_phone_is_required"].Value);
            else if (phone.Length < 10 || phone.Length > 10)
                context.AddFailure(localizer["the_phone_must_be_10_characters_long"].Value);
            else if (!Regex.IsMatch(phone, @"^(03|05|07|08|09)+([0-9]{8})$", RegexOptions.Singleline))
                context.AddFailure(localizer["the_phone_is_not_valid"].Value);
        });
    }

    // public static IRuleBuilderOptionsConditions<T, string> NotEmail<T>(this IRuleBuilder<T, string> ruleBuilder,
    //     IStringLocalizer<Resources> localizer)
    // {
    //     return ruleBuilder.Custom((email, context) =>
    //     {
    //         RuleFor(u => u.Email).EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage("The email is invalid.");
    //     });
    // }
}
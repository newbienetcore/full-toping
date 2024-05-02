using Catalog.Application.DTOs;
using Catalog.Application.Mappings;
using Catalog.Application.Properties;
using Catalog.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.Supplier })]
public class CreateSupplierCommand : BaseInsertCommand<SupplierDto>, IMapFrom<Supplier>
{
    public string Name { get; init; }
    public string Alias { get; set; }
    public string Description { get; init; }
    public string Delegate { get; init; }
    public string Email { get; init; }
    public string Bank { get; init; }
    public string AccountNumber { get; init; }
    public string BankAddress { get; init; }
    public string AddressOne { get; init; }
    public string AddressTwo { get; init; }
    public string Phone { get; init; }
    public string Fax { get; init; }
    public string TaxCode { get; init; }
    public string NationCode { get; init; }
    public string ProvinceCode { get; init; }
    public string DistrictCode { get; init; }
    public bool Status { get; init; } = true;
}

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator(IStringLocalizer<Resources> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["supplier_name_required"].Value)
            .MaximumLength(255).WithMessage(localizer["supplier_name_max_length_255"].Value);
        
        RuleFor(x => x.Delegate)
            .NotEmpty().WithMessage(localizer["supplier_delegate_required"].Value)
            .MaximumLength(500).WithMessage(localizer["supplier_delegate_max_length_50"].Value);

        RuleFor(x => x.Email)
            .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible).WithMessage(localizer["the_email_is_invalid"].Value);
        
        RuleFor(x => x.Bank)
            .MaximumLength(50).WithMessage(localizer["bank_max_length_50"].Value);
        
        RuleFor(x => x.AccountNumber)
            .MaximumLength(50).WithMessage(localizer["account_number_max_length_50"].Value)
            .Matches("^[0-9]+$").WithMessage(localizer["account_number_digits_only"].Value);
        
        RuleFor(x => x.BankAddress)
            .MaximumLength(255).WithMessage(localizer["bank_address_max_length_255"].Value);
        
        RuleFor(x => x.AddressOne)
            .NotEmpty().WithMessage(localizer["supplier_address_required"].Value)
            .MaximumLength(255).WithMessage(localizer["supplier_address_max_length_255"].Value);

        RuleFor(x => x.AddressTwo)
            .MaximumLength(255).WithMessage(localizer["supplier_address_max_length_255"].Value);
        
        RuleFor(x => x.Phone).NotPhone(localizer);
        
        RuleFor(x => x.Fax)
            .MaximumLength(50).WithMessage(localizer["fax_max_length_50"].Value);
        
        RuleFor(x => x.NationCode)
            .MaximumLength(10).WithMessage(localizer["nation_code_max_length_10"].Value);
        
        RuleFor(x => x.ProvinceCode)
            .MaximumLength(10).WithMessage(localizer["province_code_max_length_10"].Value);
        
        RuleFor(x => x.DistrictCode)
            .MaximumLength(10).WithMessage(localizer["district_code_max_length_10"].Value);
    }
}
using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Mappings;
using Catalog.Application.Properties;
using Catalog.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SharedKernel.Application;
using SharedKernel.Libraries;

namespace Catalog.Application.Features.VersionOne;

// [AuthorizationRequest(new ActionExponent[] { ActionExponent.Supplier })]
[AuthorizationRequest(new ActionExponent[] { ActionExponent.AllowAnonymous })]
public class UpdateSupplierCommand : BaseUpdateCommand, IMapFrom<Supplier>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }
    public string Description { get; set; }
    public string Delegate { get; set; }
    public string Email { get; set; }
    public string Bank { get; set; }
    public string AccountNumber { get; set; }
    public string BankAddress { get; set; }
    public string AddressOne { get; set; }
    public string AddressTwo { get; set; }
    public string Phone { get; set; }
    public string Fax { get; set; }
    public string TaxCode { get; set; }
    public string NationCode { get; set; }
    public string ProvinceCode { get; set; }
    public string DistrictCode { get; set; }
    public bool Status { get; set; } = true;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateSupplierCommand, Supplier>()
            .IgnoreAllNonExisting();
    }
    
}

public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator(IStringLocalizer<Resources> localizer)
    {
        // Tên nhà cung cấp
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(localizer["supplier_name_required"].Value)
            .MaximumLength(255).WithMessage(localizer["supplier_name_max_length_255"].Value);

        // Người đại diện
        RuleFor(x => x.Delegate)
            .NotEmpty().WithMessage(localizer["supplier_delegate_required"].Value)
            .MaximumLength(500).WithMessage(localizer["supplier_delegate_max_length_50"].Value);

        // Ngân hàng
        RuleFor(x => x.Bank)
            .MaximumLength(50).WithMessage(localizer["bank_max_length_50"].Value);

        // Số tài khoản ngân hàng
        RuleFor(x => x.AccountNumber)
            .MaximumLength(50).WithMessage(localizer["account_number_max_length_50"].Value);

        // Địa chỉ ngân hàng
        RuleFor(x => x.BankAddress)
            .MaximumLength(255).WithMessage(localizer["bank_address_max_length_255"].Value);

        // Địa chỉ một
        RuleFor(x => x.AddressOne)
            .NotEmpty().WithMessage(localizer["supplier_address_required"].Value)
            .MaximumLength(255).WithMessage(localizer["supplier_address_max_length_255"].Value);

        // Địa chỉ hai
        RuleFor(x => x.AddressTwo)
            .MaximumLength(255).WithMessage(localizer["supplier_address_max_length_255"].Value);

        // Điện thoại
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage(localizer["phone_required"])
            .MaximumLength(20).WithMessage(localizer["phone_max_length_20"].Value);

        // Fax
        RuleFor(x => x.Fax)
            .MaximumLength(50).WithMessage(localizer["fax_max_length_50"].Value);

        // Mã quốc gia
        RuleFor(x => x.NationCode)
            .MaximumLength(10).WithMessage(localizer["nation_code_max_length_10"].Value);

        // Mã tỉnh/thành phố
        RuleFor(x => x.ProvinceCode)
            .MaximumLength(10).WithMessage(localizer["province_code_max_length_10"].Value);

        // Mã quận/huyện
        RuleFor(x => x.DistrictCode)
            .MaximumLength(10).WithMessage(localizer["district_code_max_length_10"].Value);
    }
}
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Contracts;

public class ValidateField
{
    public string FieldName { get; set; }

    public ValidateCode Code { get; set; }

    public string ErrorMessage { get; set; }
}
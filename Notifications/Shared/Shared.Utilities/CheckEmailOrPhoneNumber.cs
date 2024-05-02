using System.Text.RegularExpressions;
using Shared.Utilities.Constants;

namespace Shared.Utilities;

public class CheckEmailOrPhoneNumber
{
    public static bool IsEmail(string input)
    {
        var regexEmail = new Regex(UtilitiesConstants.RegexEmailRfc5322);
        return regexEmail.IsMatch($"{input}");
    }

    public static bool IsPhoneNumber(string input)
    {
        var regexPhoneNumber = new Regex(UtilitiesConstants.RegexPhoneNumber);
        return regexPhoneNumber.IsMatch($"{input}");
    }
}
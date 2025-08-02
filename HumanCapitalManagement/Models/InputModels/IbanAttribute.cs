using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.RegularExpressions;

namespace HumanCapitalManagement.Models.InputModels;

public class IbanAttribute : ValidationAttribute
{
    private static readonly Regex IbanRegex = new Regex(@"^[A-Z]{2}[0-9]{2}[A-Z0-9]{11,30}$", RegexOptions.Compiled);

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string iban)
        {
            iban = iban.Replace(" ", "").ToUpper();

            if (!IbanRegex.IsMatch(iban))
                return new ValidationResult("IBAN format is invalid.");

            if (!IsValidIbanChecksum(iban))
                return new ValidationResult("IBAN checksum is invalid.");
        }

        return ValidationResult.Success;
    }

    private bool IsValidIbanChecksum(string iban)
    {
        string rearranged = iban.Substring(4) + iban.Substring(0, 4);

        var numericIban = "";
        foreach (char c in rearranged)
        {
            numericIban += char.IsLetter(c) ? (c - 'A' + 10).ToString() : c.ToString();
        }

        var remainder = BigInteger.Parse(numericIban) % 97;
        return remainder == 1;
    }
}
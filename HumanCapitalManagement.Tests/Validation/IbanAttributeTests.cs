using System.ComponentModel.DataAnnotations;
using HumanCapitalManagement.Models.InputModels;

namespace HumanCapitalManagement.Tests.Validation
{
    public class IbanAttributeTests
    {
        private readonly IbanAttribute _ibanAttribute = new IbanAttribute();

        [Theory]
        [InlineData("BG80BNBG96611020345678")] // валиден IBAN (България)
        [InlineData("DE89370400440532013000")] // валиден IBAN (Германия)
        [InlineData("GB29NWBK60161331926819")] // валиден IBAN (Великобритания)
        public void IsValid_ValidIban_ReturnsSuccess(string iban)
        {
            var result = _ibanAttribute.GetValidationResult(iban, new ValidationContext(new object()));

            Assert.Equal(ValidationResult.Success, result);
        }

        [Theory]
        [InlineData("BG80BNBG96611")] // твърде къс
        [InlineData("BG80BNBG966110203456789012345678901234567890")] // твърде дълъг
        [InlineData("BG80BNBG96611!@#")] // невалидни символи
        public void IsValid_InvalidFormat_ReturnsFormatError(string iban)
        {
            var result = _ibanAttribute.GetValidationResult(iban, new ValidationContext(new object()));

            Assert.NotNull(result);
            Assert.Equal("IBAN format is invalid.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_InvalidChecksum_ReturnsChecksumError()
        {
            var invalidChecksumIban = "BG80BNBG96611020345679"; // променен последен символ

            var result = _ibanAttribute.GetValidationResult(invalidChecksumIban, new ValidationContext(new object()));

            Assert.NotNull(result);
            Assert.Equal("IBAN checksum is invalid.", result.ErrorMessage);
        }

        [Fact]
        public void IsValid_NullOrEmptyValue_ReturnsSuccess()
        {
            var nullResult = _ibanAttribute.GetValidationResult(null, new ValidationContext(new object()));
            var emptyResult = _ibanAttribute.GetValidationResult("", new ValidationContext(new object()));

            Assert.Equal(ValidationResult.Success, nullResult);
            Assert.Equal("IBAN format is invalid.", emptyResult.ErrorMessage);
        }
    }
}

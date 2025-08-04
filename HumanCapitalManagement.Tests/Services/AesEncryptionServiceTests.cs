using HumanCapitalManagement.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HumanCapitalManagement.Tests.Services
{
    public class AesEncryptionServiceTests
    {
        private readonly AesEncryptionService _encryptionService;

        public AesEncryptionServiceTests()
        {
            var configMock = new Mock<IConfiguration>();

            string key = "1234567890ABCDEF";
            string iv = "FEDCBA0987654321";

            configMock.Setup(c => c["EncryptionKey"]).Returns(key);
            configMock.Setup(c => c["EncryptionIV"]).Returns(iv);

            _encryptionService = new AesEncryptionService(configMock.Object);
        }

        [Fact]
        public void EncryptDecrypt_ShouldReturnOriginalText()
        {
            var original = "This is a secret";

            var encrypted = _encryptionService.Encrypt(original);
            var decrypted = _encryptionService.Decrypt(encrypted);

            Assert.Equal(original, decrypted);
        }

        [Fact]
        public void Encrypt_ShouldReturnDifferentStringThanInput()
        {
            var plainText = "Another message";

            var encrypted = _encryptionService.Encrypt(plainText);

            Assert.NotEqual(plainText, encrypted);
            Assert.False(string.IsNullOrEmpty(encrypted));
        }

        [Fact]
        public void Decrypt_InvalidData_ShouldThrow()
        {
            var invalid = "invalid_base64!";

            Assert.ThrowsAny<Exception>(() => _encryptionService.Decrypt(invalid));
        }
    }
}
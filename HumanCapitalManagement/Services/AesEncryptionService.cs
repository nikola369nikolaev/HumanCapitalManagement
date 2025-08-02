using System.Security.Cryptography;
using System.Text;

namespace HumanCapitalManagement.Services;

public class AesEncryptionService : IAesEncryptionService
{
    private readonly IConfiguration _configuration;

    public AesEncryptionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_configuration["EncryptionKey"]);
        aes.IV = Encoding.UTF8.GetBytes(_configuration["EncryptionIV"]);

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string encryptedText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_configuration["EncryptionKey"]);
        aes.IV = Encoding.UTF8.GetBytes(_configuration["EncryptionIV"]);

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(Convert.FromBase64String(encryptedText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
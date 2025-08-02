namespace HumanCapitalManagement.Services;

public interface IAesEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string encryptedText);
}
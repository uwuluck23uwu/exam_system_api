using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace EXAM_SYSTEM_API.API.Shared
{
  public class EncryptJsonResponse
  {
    //------------------------------------------ เข้ารหัส Json Response ------------------------------------------//

    // Helper method to pad or truncate the key to the desired size
    private static string PadOrTruncateKey(string key, int desiredSize)
    {
      if (key.Length < desiredSize)
      {
        // Pad the key with zeroes
        key = key.PadRight(desiredSize, '\0');
      }
      else if (key.Length > desiredSize)
      {
        // Truncate the key
        key = key.Substring(0, desiredSize);
      }
      return key;
    }

    // Encrypt JSON data using AES encryption algorithm
    public string EncryptJson(object jsonData, string encryptionKey)
    {
      var settings = new JsonSerializerSettings
      {
        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
      };
      string jsonString = JsonConvert.SerializeObject(jsonData, settings);
      return EncryptJson(jsonString, encryptionKey);
    }

    // Decrypt JSON data using AES encryption algorithm
    public T DecryptJson<T>(string encryptedData, string encryptionKey)
    {
      string decryptedJsonString = DecryptJson(encryptedData, encryptionKey);
      return System.Text.Json.JsonSerializer.Deserialize<T>(decryptedJsonString)!;
    }

    // Encrypt data using AES encryption algorithm
    private string EncryptJson(string data, string encryptionKey)
    {
      try
      {
        // Ensure the encryption key is 256 bits long (32 bytes)
        encryptionKey = PadOrTruncateKey(encryptionKey, 32);

        using (Aes aesAlg = Aes.Create())
        {
          aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
          aesAlg.IV = new byte[16]; // IV should be randomly generated, but for simplicity, using all zeros here

          // Create an encryptor to perform the stream transform
          ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

          // Create the streams used for encryption
          using (MemoryStream msEncrypt = new MemoryStream())
          {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
              using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
              {
                // Write all data to the stream
                swEncrypt.Write(data);
              }
            }
            // Convert encrypted data to a base64 string
            return Convert.ToBase64String(msEncrypt.ToArray());
          }
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return "";
      }
    }

    // Decrypt data using AES encryption algorithm
    private string DecryptJson(string encryptedData, string encryptionKey)
    {
      try
      {
        // Ensure the encryption key is 256 bits long (32 bytes)
        encryptionKey = PadOrTruncateKey(encryptionKey, 32);

        byte[] cipherText = Convert.FromBase64String(encryptedData);

        using (Aes aesAlg = Aes.Create())
        {
          aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
          aesAlg.IV = new byte[16]; // IV should be randomly generated, but for simplicity, using all zeros here

          // Create a decryptor to perform the stream transform
          ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

          // Create the streams used for decryption
          using (MemoryStream msDecrypt = new MemoryStream(cipherText))
          {
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
              using (StreamReader srDecrypt = new StreamReader(csDecrypt))
              {
                // Read the decrypted bytes from the decrypting stream
                // and place them in a string
                return srDecrypt.ReadToEnd();
              }
            }
          }
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return "";
      }
    }

    //------------------------------------------ เข้ารหัส Json Response ------------------------------------------//
  }
}

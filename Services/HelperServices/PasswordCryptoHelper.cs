using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Services.HelperServices;

public class PasswordCryptoHelper
{
	private readonly string secretkey;
	public PasswordCryptoHelper(IConfiguration configuration)
	{
		secretkey = configuration.GetSection("CryptoHashKey").Value;
	}
	public string GenerateEmailToken(string email)
	{
		Random random = new Random();
		var getRandomNumber = random.Next(100000, 999999);
		string combinedString = email + getRandomNumber;
		using (SHA256 sha256 = SHA256.Create())
		{
			byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
			string token = Convert.ToBase64String(hashedBytes); // alınan email ve random sayı ile beraber şifreleme
			return token;
		}
	}
	public string EncryptString(string input)
	{
		var key = Encoding.UTF8.GetBytes(secretkey);

		using (var aesAlg = Aes.Create())
		{
			aesAlg.Mode = CipherMode.CBC;
			aesAlg.Padding = PaddingMode.PKCS7;

			using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
			{
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(input);
						}
					}

					var iv = aesAlg.IV;
					var encryptedContent = msEncrypt.ToArray();

					var result = new byte[iv.Length + encryptedContent.Length];

					Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
					Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);
					var resultUrl = Convert.ToBase64String(result);
					return resultUrl;

				}
			}
		}
	}
	public string DecryptString(string cipherText)
	{

		var fullCipher = Convert.FromBase64String(cipherText);

		var iv = new byte[16];
		var cipher = new byte[fullCipher.Length - iv.Length];

		Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
		Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

		var key = Encoding.UTF8.GetBytes(secretkey);

		using (var aesAlg = Aes.Create())
		{
			aesAlg.Mode = CipherMode.CBC;
			aesAlg.Padding = PaddingMode.PKCS7;

			using (var decryptor = aesAlg.CreateDecryptor(key, iv))
			{
				string result;
				using (var msDecrypt = new MemoryStream(cipher))
				{
					using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (var srDecrypt = new StreamReader(csDecrypt))
						{
							result = srDecrypt.ReadToEnd();
						}
					}
				}

				return result;
			}
		}

	}
}
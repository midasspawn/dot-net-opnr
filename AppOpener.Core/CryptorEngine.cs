using System;
using System.Security.Cryptography;
using System.Text;

namespace AppOpener.Core
{
	public static class CryptorEngine
	{
		public static string Encrypt(string toEncrypt, bool useHashing = true)
		{
			byte[] keyArray;
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

			// Get the key from config file
			string key = "cpm_Iotasol_Project+-!5663a#KN";

			if (useHashing)
			{
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
				hashmd5.Clear();
			}
			else
				keyArray = UTF8Encoding.UTF8.GetBytes(key);

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = tdes.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			tdes.Clear();
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}
		/// <summary>
		/// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
		/// </summary>
		/// <param name="cipherString">encrypted string</param>
		/// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
		/// <returns></returns>
		public static string Decrypt(string cipherString, bool useHashing = true)
		{
			try
			{
				cipherString = cipherString.Replace(" ", "+");
				byte[] keyArray;
				byte[] toEncryptArray = Convert.FromBase64String(cipherString);

				//Get your key from config file to open the lock!
				string key = "cpm_Iotasol_Project+-!5663a#KN";

				if (useHashing)
				{
					MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
					keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
					hashmd5.Clear();
				}
				else
					keyArray = UTF8Encoding.UTF8.GetBytes(key);

				TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
				tdes.Key = keyArray;
				tdes.Mode = CipherMode.ECB;
				tdes.Padding = PaddingMode.PKCS7;

				ICryptoTransform cTransform = tdes.CreateDecryptor();
				byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

				tdes.Clear();
				return UTF8Encoding.UTF8.GetString(resultArray);
			}
			catch
			{
				return null;
			}
		}

		public static string ConvertToSHA256(string randomString)
		{
			var crypt = new System.Security.Cryptography.SHA256Managed();
			var hash = new System.Text.StringBuilder();
			byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
			foreach (byte theByte in crypto)
			{
				hash.Append(theByte.ToString("x2"));
			}
			return hash.ToString();
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AppOpener.Core
{
	public partial class CommonHelper
	{
		/// <summary>
		/// Verifies that a string is in valid e-mail format
		/// </summary>
		/// <param name="email">Email to verify</param>
		/// <returns>true if the string is a valid e-mail address and false if it's not</returns>
		public static bool IsValidEmail(string email)
		{
			bool result = false;
			if (String.IsNullOrEmpty(email))
				return result;
			email = email.Trim();
			result = Regex.IsMatch(email, StateKeyManager.EmailValidationRegEx);
			return result;
		}

		/// <summary>
		/// Verifies that a string is in valid url format
		/// </summary>
		/// <param name="url">Url to verify</param>
		/// <returns>true if the string is a valid address and false if it's not</returns>
		public static bool IsValidHref(string url)
		{
			bool result = false;
			if (String.IsNullOrEmpty(url))
				return result;
			url = url.Trim();
			result = Regex.IsMatch(url, StateKeyManager.URlValidationRegEx);
			return result;
		}

		/// <summary>
		/// Generate random digit code
		/// </summary>
		/// <param name="length">Length</param>
		/// <returns>Result string</returns>
		public static string GenerateRandomDigitCode(int length)
		{
			var random = new Random();
			string str = string.Empty;
			for (int i = 0; i < length; i++)
				str = String.Concat(str, random.Next(10).ToString());
			return str;
		}

		/// <summary>
		/// Returns an random interger number within a specified rage
		/// </summary>
		/// <param name="min">Minimum number</param>
		/// <param name="max">Maximum number</param>
		/// <returns>Result</returns>
		public static int GenerateRandomInteger(int min = 0, int max = 2147483647)
		{
			var randomNumberBuffer = new byte[10];
			new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
			return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
		}

		/// <summary>
		/// Ensure that a string doesn't exceed maximum allowed length
		/// </summary>
		/// <param name="str">Input string</param>
		/// <param name="maxLength">Maximum length</param>
		/// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
		public static string EnsureMaximumLength(string str, int maxLength)
		{
			if (String.IsNullOrEmpty(str))
				return str;

			if (str.Length > maxLength)
				return str.Substring(0, maxLength);
			else
				return str;
		}

		/// <summary>
		/// Ensures that a string only contains numeric values
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
		public static string EnsureNumericOnly(string str)
		{
			if (String.IsNullOrEmpty(str))
				return string.Empty;

			var result = new StringBuilder();
			foreach (char c in str)
			{
				if (Char.IsDigit(c))
					result.Append(c);
			}
			return result.ToString();
		}

		/// <summary>
		/// Ensure that a string is not null
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>Result</returns>
		public static string EnsureNotNull(string str)
		{
			if (str == null)
				return string.Empty;

			return str;
		}

		/// <summary>
		/// Indicates whether the specified strings are null or empty strings
		/// </summary>
		/// <param name="stringsToValidate">Array of strings to validate</param>
		/// <returns>Boolean</returns>
		public static bool AreNullOrEmpty(params string[] stringsToValidate)
		{
			bool result = false;
			Array.ForEach(stringsToValidate, str =>
			{
				if (string.IsNullOrEmpty(str)) result = true;
			});
			return result;
		}



		/// <summary>
		/// Strips all illegal characters from the specified title.
		/// </summary>
		/// <param name="text">
		/// The text to strip.
		/// </param>
		/// <returns>
		/// The remove illegal characters.
		/// </returns>
		public static string RemoveIllegalCharacters(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}

			text = text.Replace(":", string.Empty);
			text = text.Replace("/", string.Empty);
			text = text.Replace("?", string.Empty);
			text = text.Replace("#", string.Empty);
			text = text.Replace("[", string.Empty);
			text = text.Replace("]", string.Empty);
			text = text.Replace("@", string.Empty);
			text = text.Replace("*", string.Empty);
			text = text.Replace(".", string.Empty);
			text = text.Replace(",", string.Empty);
			text = text.Replace("\"", string.Empty);
			text = text.Replace("&", string.Empty);
			text = text.Replace("'", string.Empty);
			text = text.Replace(" ", "-");
			text = RemoveDiacritics(text);
			text = RemoveExtraHyphen(text);

			return HttpUtility.HtmlEncode(text).Replace("%", string.Empty);
		}

		/// <summary>
		/// Removes the diacritics.
		/// </summary>
		/// <param name="text">
		/// The text to remove diacritics from.
		/// </param>
		/// <returns>
		/// The string with the diacritics removed.
		/// </returns>
		private static string RemoveDiacritics(string text)
		{
			var normalized = text.Normalize(NormalizationForm.FormD);
			var sb = new StringBuilder();

			foreach (var c in
				normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
			{
				sb.Append(c);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Removes the extra hyphen.
		/// </summary>
		/// <param name="text">
		/// The text to remove the extra hyphen from.
		/// </param>
		/// <returns>
		/// The text with the extra hyphen removed.
		/// </returns>
		private static string RemoveExtraHyphen(string text)
		{
			if (text.Contains("--"))
			{
				text = text.Replace("--", "-");
				return RemoveExtraHyphen(text);
			}

			return text;
		}

		/// <summary>
		/// Converts the passed string to proper case 
		/// </summary>
		/// <example>rajeev ranjan will be converted to Rajeev Ranjan</example>
		/// <param name="Value"></param>
		/// <returns></returns>
		public static string ProperCase(string Value)
		{
			StringBuilder sb = new StringBuilder();
			string[] words = Value.Split(new char[] { ' ' });

			foreach (string word in words)
			{
				sb.Append(Char.ToUpper(word[0])); // first letter
				sb.Append(word.Substring(1).ToLower()); // remaining words
				sb.Append(" ");
			}

			return sb.ToString();
		}
		public static TypeConverter GetCustomTypeConverter(Type type)
		{
			//we can't use the following code in order to register our custom type descriptors
			//TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
			//so we do it manually here

			if (type == typeof(List<int>))
				return new GenericListTypeConverter<int>();
			if (type == typeof(List<decimal>))
				return new GenericListTypeConverter<decimal>();
			if (type == typeof(List<string>))
				return new GenericListTypeConverter<string>();
			if (type == typeof(List<Guid>))
				return new GenericListTypeConverter<Guid>();

			return TypeDescriptor.GetConverter(type);
		}
		public static object To(object value, Type destinationType)
		{
			return To(value, destinationType, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Converts a value to a destination type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The type to convert the value to.</param>
		/// <param name="culture">Culture</param>
		/// <returns>The converted value.</returns>
		public static object To(object value, Type destinationType, CultureInfo culture)
		{
			if (value != null)
			{
				var sourceType = value.GetType();

				TypeConverter destinationConverter = GetCustomTypeConverter(destinationType);
				TypeConverter sourceConverter = GetCustomTypeConverter(sourceType);
				if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
					return destinationConverter.ConvertFrom(null, culture, value);
				if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
					return sourceConverter.ConvertTo(null, culture, value, destinationType);
				if (destinationType.IsEnum && value is int)
					return Enum.ToObject(destinationType, (int)value);
				if (!destinationType.IsAssignableFrom(value.GetType()))
					return Convert.ChangeType(value, destinationType, culture);
			}
			return value;
		}
		/// <summary>
		/// Converts a value to a destination type.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <typeparam name="T">The type to convert the value to.</typeparam>
		/// <returns>The converted value.</returns>
		public static T To<T>(object value)
		{
			//return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
			return (T)To(value, typeof(T));
		}

		/// <summary>
		/// Convert enum for front-end
		/// </summary>
		/// <param name="str">Input string</param>
		/// <returns>Converted string</returns>
		public static string ConvertEnum(string str)
		{
			string result = string.Empty;
			char[] letters = str.ToCharArray();
			foreach (char c in letters)
				if (c.ToString() != c.ToString().ToLower())
					result += " " + c.ToString();
				else
					result += c.ToString();
			return result;
		}

		/// <summary>
		/// Convert a byte array to string
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ByteArrayToString(byte[] input)
		{
			var enc = new ASCIIEncoding();
			return enc.GetString(input);
		}

		/// <summary>
		/// String to byte array
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static byte[] StringToByteArray(string input)
		{
			var encoding = new ASCIIEncoding();
			return encoding.GetBytes(input);
		}

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}
		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		public static string GenerateRandomOTP(int minLength = 8, bool isAlphanumeric = true)
		{
			string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
			string numbers = "1234567890";

			string characters = numbers;
			if (isAlphanumeric)
			{
				characters += alphabets + small_alphabets + numbers;
			}
			int length = minLength;

			string otp = string.Empty;
			for (int i = 0; i < length; i++)
			{
				string character = string.Empty;
				do
				{
					int index = new Random().Next(0, characters.Length);
					character = characters.ToCharArray()[index].ToString();
				} while (otp.IndexOf(character) != -1);
				otp += character;
			}
			return otp;
		}
		public static string Replace(string template, IEnumerable<Token> tokens, bool htmlEncode)
		{
			if (string.IsNullOrWhiteSpace(template))
				throw new ArgumentNullException("template");

			if (tokens == null)
				throw new ArgumentNullException("tokens");

			foreach (var token in tokens)
			{
				string tokenValue = token.Value;
				//do not encode URLs
				if (htmlEncode && !token.NeverHtmlEncoded)
					tokenValue = HttpUtility.HtmlEncode(tokenValue);
				template = Replace(template, String.Format(@"%{0}%", token.Key), tokenValue);
			}
			return template;
		}
		private static string Replace(string original, string pattern, string replacement)
		{
			return original.Replace(pattern, replacement);
		}

		public static double GetPriceWithTax(string price, int noOfQuantity = 1, double tax = 0)
		{
			if (!string.IsNullOrEmpty(price))
			{
				try
				{
					var originalPrice = Convert.ToDouble(price) * noOfQuantity;

					return originalPrice + ((originalPrice * tax) / 100);
				}
				catch
				{
					return 0;
				}
			}
			else
				return 0;
		}
		public static double GetDiscountedPrice(string price, int noOfQuantity = 1, double tax = 0, double discount = 0)
		{
			if (!string.IsNullOrEmpty(price))
			{
				try
				{
					var originalPrice = Convert.ToDouble(price) * noOfQuantity;
					var discountedPrice = originalPrice - ((originalPrice * discount) / 100);

					return discountedPrice + ((discountedPrice * tax) / 100);
				}
				catch
				{
					return 0;
				}
			}
			else
				return 0;
		}
		public static int ConvertToLeastUnit(double instance)
		{
			return Convert.ToInt32(instance * 100);
		}
		public static string GenerateRandomReceipt()
		{
			return string.Format("rcptid_{0}", DateTime.Now.ToString("yyMMddHHmmssff").Replace("-", "").ToLower());
		}
		public static string HmacSha256Digest(string message, string secret)
		{
			ASCIIEncoding encoding = new ASCIIEncoding();
			byte[] keyBytes = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			System.Security.Cryptography.HMACSHA256 cryptographer = new System.Security.Cryptography.HMACSHA256(keyBytes);

			byte[] bytes = cryptographer.ComputeHash(messageBytes);

			return BitConverter.ToString(bytes).Replace("-", "").ToLower();
		}

		public static string GenerateUniqueId(int length = 10)
		{
			Guid g = Guid.NewGuid();
			string GuidString = Convert.ToBase64String(g.ToByteArray());
			GuidString = GuidString.Replace("=", "");
			GuidString = GuidString.Replace("+", "");

			return GuidString.Substring(0, length);
		}
		public static string GenerateUniqueNumber(int length = 10)
		{
			Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			return unixTimestamp.ToString().Substring(0, length);
		}

		public static double CalculateVolumetricWeight(double length, double width, double height)
		{
			var result = ((length * width * height) / 5000);

			return Math.Round(result, 2);
		}

		public static string MaskEmail(string input)
		{
			if (!string.IsNullOrEmpty(input))
			{
				string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
				string result = Regex.Replace(input, pattern, m => new string('*', m.Length));

				return result;
			}
			else
				return input;
		}

		public static int ConvertKgToGm(double input)
		{
			var result = 0;

			var weightinkg = input * 1000;

			result = Convert.ToInt32(weightinkg);
			return result;
		}

		public static string ConvertActionType(int TypeId)
		{
			var result = string.Empty;
			if (TypeId == 1)
				result = "NDR Raised";
			if (TypeId == 2)
				result = "Re-Attempt";
			if (TypeId == 3)
				result = "Change Address";
			if (TypeId == 4)
				result = "Change Contact Number";
			if (TypeId == 5)
				result = "RTO";
			if (TypeId == 6)
				result = "Fake Attempt";

			return result;
		}

		public static string ConvertServiceProviderMode(int ModeId)
		{
			var result = string.Empty;
			if (ModeId == 1)
				result = "Road";
			if (ModeId == 2)
				result = "Air";
			if (ModeId == 3)
				result = "Water";

			return result;
		}
		public static string ConvertServiceProviderType(int TypeId)
		{
			var result = string.Empty;
			if (TypeId == 1)
				result = "Normal";
			if (TypeId == 2)
				result = "Express";


			return result;
		}
		public static string GetBulkOrderActivity(int TypeId)
		{
			var result = string.Empty;

			if (TypeId == (int)BulkOrderRequestType.ExcelCreateOrder)
				result = "Bulk Order";


			return result;
		}

		public static string GetBulkOrderActivityStatus(int? status)
		{
			var result = string.Empty;

			if (status.HasValue)
			{
				if (status.Value == (int)BulkOrderRequestStatus.Completed)
					result = "Completed";
				else if (status.Value == (int)BulkOrderRequestStatus.CompletedWithError)
					result = "Partial Completed";
				else if (status.Value == (int)BulkOrderRequestStatus.Exception)
					result = "Unknown Error! Please contact system administrator.";
				else
					result = "Pending";
			}
			else
			{
				result = "Pending";
			}

			return result;
		}
	}
}

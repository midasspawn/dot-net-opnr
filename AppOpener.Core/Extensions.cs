using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace AppOpener.Core
{
	public static class Extensions
	{
		public static bool IsNullOrDefault<T>(this T? value) where T : struct
		{
			return default(T).Equals(value.GetValueOrDefault());
		}

		public static string FormatWith(this string instance, params object[] args)
		{
			return string.Format(CultureInfo.CurrentCulture, instance, args);
		}

		public static bool HasValue(this string value)
		{
			return !string.IsNullOrEmpty(value);
		}

		public static bool IsCaseInsensitiveEqual(this string instance, string comparing)
		{
			return (string.Compare(instance, comparing, StringComparison.OrdinalIgnoreCase) == 0);
		}

		public static bool IsCaseSensitiveEqual(this string instance, string comparing)
		{
			return (string.CompareOrdinal(instance, comparing) == 0);
		}

		public static bool IsEmpty(this Guid value)
		{
			return value.Equals(Guid.Empty);
		}
		public static bool IsEmpty(this Int64 value)
		{
			if (value == 0)
				return true;
			else
				return false;
		}


		public static string RemoveDashes(this Guid value)
		{
			return value.ToString().Replace("-", string.Empty);
		}

		public static string Compress(this string instance)
		{
			byte[] buffer;
			Guard.IsNotNullOrEmpty(instance, "instance");
			byte[] bytes = Encoding.UTF8.GetBytes(instance);
			using (MemoryStream stream = new MemoryStream())
			{
				using (GZipStream zipstream = new GZipStream(stream, CompressionMode.Compress))
				{
					zipstream.Write(bytes, 0, bytes.Length);
				}
				buffer = stream.ToArray();
			}
			byte[] dst = new byte[buffer.Length + 4];
			Buffer.BlockCopy(buffer, 0, dst, 4, buffer.Length);
			Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, dst, 0, 4);
			return Convert.ToBase64String(dst);
		}

		public static string Decompress(this string instance)
		{
			string str;
			Guard.IsNotNullOrEmpty(instance, "instance");
			byte[] buffer = Decode(instance);
			if (buffer.Length < 4)
			{
				return string.Empty;
			}
			using (MemoryStream stream = new MemoryStream())
			{
				int num = BitConverter.ToInt32(buffer, 0);
				stream.Write(buffer, 4, buffer.Length - 4);
				byte[] buffer2 = new byte[num];
				stream.Seek(0L, SeekOrigin.Begin);
				GZipStream stream2 = new GZipStream(stream, CompressionMode.Decompress);
				try
				{
					stream2.Read(buffer2, 0, buffer2.Length);
					str = Encoding.UTF8.GetString(buffer2);
				}
				catch (InvalidDataException)
				{
					str = string.Empty;
				}
				finally
				{
					if (stream2 != null)
					{
						stream2.Dispose();
					}
				}
			}
			return str;
		}


		public static Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>> Pivot<TSource, TFirstKey, TSecondKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TFirstKey> firstKeySelector, Func<TSource, TSecondKey> secondKeySelector, Func<IEnumerable<TSource>, TValue> aggregate)
		{
			var retVal = new Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>();

			var l = source.ToLookup(firstKeySelector);
			foreach (var item in l)
			{
				var dict = new Dictionary<TSecondKey, TValue>();
				retVal.Add(item.Key, dict);
				var subdict = item.ToLookup(secondKeySelector);
				foreach (var subitem in subdict)
				{
					dict.Add(subitem.Key, aggregate(subitem));
				}
			}

			return retVal;
		}

		private static byte[] Decode(string value)
		{
			try
			{
				return Convert.FromBase64String(value);
			}
			catch (FormatException)
			{
				return new byte[0];
			}
		}



		public static DataTable ToPivotTable<T, TColumn, TRow, TData>(
								this IEnumerable<T> source,
								Func<T, TColumn> columnSelector,
								Expression<Func<T, TRow>> rowSelector,
								Func<IEnumerable<T>, TData> dataSelector)
		{
			DataTable table = new DataTable();
			var rowName = ((MemberExpression)rowSelector.Body).Member.Name;
			table.Columns.Add(new DataColumn(rowName));
			var columns = source.Select(columnSelector).Distinct();

			foreach (var column in columns)
				table.Columns.Add(new DataColumn(column.ToString()));

			var rows = source.GroupBy(rowSelector.Compile())
							 .Select(rowGroup => new
							 {
								 Key = rowGroup.Key,
								 Values = columns.GroupJoin(
									 rowGroup,
									 c => c,
									 r => columnSelector(r),
									 (c, columnGroup) => dataSelector(columnGroup))
							 });

			foreach (var row in rows)
			{
				var dataRow = table.NewRow();
				var items = row.Values.Cast<object>().ToList();
				items.Insert(0, row.Key);
				dataRow.ItemArray = items.ToArray();
				table.Rows.Add(dataRow);
			}

			return table;
		}
		private static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
		{
			T obj = new T();
			try
			{
				string columnname = "";
				string value = "";
				PropertyInfo[] Properties;
				Properties = typeof(T).GetProperties();
				foreach (PropertyInfo objProperty in Properties)
				{
					columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
					if (!string.IsNullOrEmpty(columnname))
					{
						value = row[columnname].ToString();
						if (!string.IsNullOrEmpty(value))
						{
							if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
							{
								value = row[columnname].ToString().Replace("$", "").Replace(",", "");
								objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
							}
							else
							{
								value = row[columnname].ToString().Replace("%", "");
								objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
							}
						}
					}
				}
				return obj;
			}
			catch
			{
				return obj;
			}
		}

		public static string Titleize(this string text)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text).ToSentenceCase();
		}

		public static string ToSentenceCase(this string str)
		{
			return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
		}

		public static string ReplaceNumericWithPercentage(this string str)
		{
			if (!string.IsNullOrEmpty(str))
				return Regex.Replace(str, @"\d+", m => "(" + (m.Groups[0].Value.ToString() + "%)")).Replace(";", ", ");
			else
				return str;
		}
		public static Dictionary<string, string> ConvertToList(this string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				var words = str;

				var keywords = Regex.Replace(words, @"\d+", m => string.Empty);
				var keylist = keywords.Split(';');

				var statement = Regex.Replace(words, @"\d+", m => "(" + (m.Groups[0].Value.ToString() + "%)"));
				var valuelist = statement.Split(';');

				var result = new Dictionary<string, string>();

				for (int i = 0; i < keylist.Length; i++)
				{
					result.Add(keylist[i].Trim().ToString(), valuelist[i].ToString());
				}

				return result;
			}
			else
				return new Dictionary<string, string>();

		}

		public static string ToNewFileName(this string file)
		{
			var extension = Path.GetExtension(file);
			var filename = Path.GetFileNameWithoutExtension(file);

			return string.Format("{0}_{1}{2}", filename, Guid.NewGuid().ToString(), extension);
		}
		public static string RemoveSpecialCharacter(this string input)
		{
			if (!string.IsNullOrEmpty(input))
			{
				return string.Join("_", input.Split(Path.GetInvalidFileNameChars())).Replace(" ", "");
			}
			else
				return input;

		}

		public static DateTime? ToDateTime(this string input)
		{
			DateTime? date = null;
			try
			{
				if (!string.IsNullOrEmpty(input))
				{
					date = Convert.ToDateTime(input).Date;
				}
			}
			catch
			{
			}
			return date;
		}

		public static string FormatFirstLetter(this string input)
		{
			string s = input.Trim();
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1).ToLower();
		}

		public static double ToDecimalFormat(this double instance, int decimalplace = 2)
		{
			return Math.Round(instance, decimalplace);
		}

		public static string GetYouTubeVideoIdFromUrl(this string url)
		{
			Uri uri = null;
			if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
			{
				try
				{
					uri = new UriBuilder("http", url).Uri;
				}
				catch
				{
					// invalid url
					return "";
				}
			}

			string host = uri.Host;
			string[] youTubeHosts = { "www.youtube.com", "youtube.com", "youtu.be", "www.youtu.be" };
			if (!youTubeHosts.Contains(host))
				return "";

			var query = HttpUtility.ParseQueryString(uri.Query);

			if (query.AllKeys.Contains("v"))
			{
				return Regex.Match(query["v"], @"^[a-zA-Z0-9_-]{11}$").Value;
			}
			else if (query.AllKeys.Contains("u"))
			{
				// some urls have something like "u=/watch?v=AAAAAAAAA16"
				return Regex.Match(query["u"], @"/watch\?v=([a-zA-Z0-9_-]{11})").Groups[1].Value;
			}
			else
			{
				// remove a trailing forward space
				var last = uri.Segments.Last().Replace("/", "");
				if (Regex.IsMatch(last, @"^v=[a-zA-Z0-9_-]{11}$"))
					return last.Replace("v=", "");

				string[] segments = uri.Segments;
				if (segments.Length > 2 && segments[segments.Length - 2] != "v/" && segments[segments.Length - 2] != "watch/")
					return "";

				return Regex.Match(last, @"^[a-zA-Z0-9_-]{11}$").Value;
			}
		}

		public static List<T> ConvertDataTableToGenericList<T>(this DataTable dt)
		{
			const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
			var columnNames = dt.Columns.Cast<DataColumn>()
				.Select(c => c.ColumnName)
				.ToList();
			var objectProperties = typeof(T).GetProperties(flags);

			var targetList = dt.AsEnumerable().Select(dataRow =>
			{
				var instanceOfT = Activator.CreateInstance<T>();

				foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
				{
					if (properties.PropertyType == typeof(DateTime))
					{
						properties.SetValue(instanceOfT, DateTime.Parse(dataRow[properties.Name].ToString()), null);
					}
					else if (properties.PropertyType == typeof(DateTime?))
					{
						properties.SetValue(instanceOfT, (DateTime?)DateTime.Parse(dataRow[properties.Name].ToString()), null);
					}
					else
						properties.SetValue(instanceOfT, dataRow[properties.Name], null);
				}
				return instanceOfT;
			}).ToList();

			return targetList;
		}
		public static List<T> ToListof<T>(this DataTable dt)
		{
			const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
			var columnNames = dt.Columns.Cast<DataColumn>()
				.Select(c => c.ColumnName)
				.ToList();
			var objectProperties = typeof(T).GetProperties(flags);

			var targetList = dt.AsEnumerable().Select(dataRow =>
			{
				var instanceOfT = Activator.CreateInstance<T>();
				foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
				{
					if (properties.PropertyType == typeof(DateTime))
					{
						properties.SetValue(instanceOfT, DateTime.Parse(dataRow[properties.Name].ToString()), null);
					}
					else if (properties.PropertyType == typeof(DateTime?))
					{
						properties.SetValue(instanceOfT, (DateTime?)DateTime.Parse(dataRow[properties.Name].ToString()), null);
					}
					else
						properties.SetValue(instanceOfT, dataRow[properties.Name], null);
				}
				return instanceOfT;
			}).ToList();

			return targetList;
		}
		public static DateTime? UtcToLocal(this DateTime? timeUtc, string timeZoneId = "India Standard Time")
		{
			if (timeUtc.HasValue)
			{
				if (!string.IsNullOrEmpty(timeZoneId))
				{
					var dtime = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);// "India Standard Time");
					DateTime destTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc.Value, dtime);
					return destTime;
				}
				else
					return timeUtc;
			}
			else
				return timeUtc;
		}

		public static DateTime UtcToLocal(this DateTime timeUtc, string timeZoneId = "India Standard Time")
		{
			if (!string.IsNullOrEmpty(timeZoneId))
			{
				var dtime = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);// "India Standard Time");
				DateTime destTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, dtime);
				return destTime;
			}
			else
				return timeUtc;

		}


		public static DateTime? LocalToUtc(this DateTime? timeUtc, string timeZoneId = "")
		{
			if (timeUtc.HasValue)
			{
				if (!string.IsNullOrEmpty(timeZoneId))
				{
					var dtime = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);// "India Standard Time");
					DateTime destTime = TimeZoneInfo.ConvertTimeToUtc(timeUtc.Value, dtime);
					return destTime;
				}
				else
					return timeUtc;
			}
			else
				return timeUtc;
		}

		public static DateTime LocalToUtc(this DateTime timeUtc, string timeZoneId = "")
		{
			if (!string.IsNullOrEmpty(timeZoneId))
			{
				var dtime = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);// "India Standard Time");
				DateTime destTime = TimeZoneInfo.ConvertTimeToUtc(timeUtc, dtime);
				return destTime;
			}
			else
				return timeUtc;

		}

		
	}
}

using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace AppOpener.Core.Validator
{
	public class AllowedExtensionsAttribute : ValidationAttribute
	{
		private readonly string[] _allowedExtensions;
		public AllowedExtensionsAttribute(string[] extensions)
		{
			_allowedExtensions = extensions;
		}

		protected override ValidationResult IsValid(
		object value, ValidationContext validationContext)
		{
			var file = value as IFormFile;
			if (file != null)
			{
				var extension = Path.GetExtension(file.FileName);
				if (!_allowedExtensions.Contains(extension.ToLower()))
				{
					return new ValidationResult(GetErrorMessage(extension));
				}
			}

			return ValidationResult.Success;
		}

		public string GetErrorMessage(string extension)
		{
			return string.Format("{0} extension is not allowed! Only files with following extensions are allowed: {1} ", extension, _allowedExtensions.ToDelimitedString(", "));
		}
	}
}

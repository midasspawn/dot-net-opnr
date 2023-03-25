using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppOpener.Core.Validator
{
	public class StringArrayValidationAttribute : ValidationAttribute
	{
		private readonly string _fieldname;
		public StringArrayValidationAttribute(string fieldname) : base("{0} is required.")
		{
			_fieldname = fieldname;
		}
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var array = value as List<string>;

			if (array != null)
			{
				var errorMessage = FormatErrorMessage((validationContext.DisplayName));
				//// if empty not valid
				if (array.Count == 0)
					return new ValidationResult(errorMessage);
			}

			return ValidationResult.Success;
		}
	}
}

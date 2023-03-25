using System;
using System.Collections.Generic;
using System.Text;

namespace AppOpener.Core
{
	public class PropertyAttribute : Attribute
	{
		public PropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		public string PropertyName { get; private set; }
	}
}

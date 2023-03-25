using System;

namespace AppOpener.Core.BusinessEntities.Configuration
{
	public partial class SettingViewModel
	{
		public SettingViewModel() { }

		public SettingViewModel(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}

		/// <summary>
		/// Gets or sets the name
		/// </summary>
		public virtual Int64 SettingId { get; set; }
		/// <summary>
		/// Gets or sets the name
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// Gets or sets the value
		/// </summary>
		public virtual string Value { get; set; }


		/// <summary>
		/// Returns the setting value as the specified type
		/// </summary>
		public virtual T As<T>()
		{
			return CommonHelper.To<T>(this.Value);
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public class CommonSetting : ISettings
	{
		public string SiteName { get; set; }
		public string SiteUrl { get; set; }
		public string SiteLogo { get; set; }
		public string StaticResourcePath { get; set; }
		public string ApiUrl { get; set; }
		public double ServiceTax { get; set; }
		public bool IsDirectShipNow { get; set; }
		public string PickupTime { get; set; }

        public bool IsApiCallNeededForOrderCancellation { get; set; }
        public int PendingOrderCancellationAfterDay { get; set; }

    }

	public class WebAppSetting : ISettings
	{
		public string AppUrl { get; set; }
		public string EmailConfirmationUrl { get; set; }
	}
	public class RazorPaySetting : ISettings
	{
		public string BaseUrl { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string ConfirmPaymentRedirectUrl { get; set; }
	}

	public class OrderSetting: ISettings
	{
		public int BulkCount { get; set; }
		public Int64 DefaultCourierId { get; set; }

		public int MaxBulkData { get; set; }
	}

	public class WebjobSetting : ISettings
	{
		public string Token { get; set; }
	}

    public class smslanesettings : ISettings
    {
        public string SmsUrl { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public string Ssid { get; set; }
    }

	public class PdfSetting : ISettings
	{
		public string LicenseKeyValue { get; set; }
		public string LicenseKey { get; set; }
	}
}

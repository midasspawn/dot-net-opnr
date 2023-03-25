using AutoMapper;
using AppOpener.Core;
using AppOpener.Core.BusinessEntities.Configuration;
using AppOpener.Data.Interfaces;
using AppOpener.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AppOpener.Services.Configuration
{
	public interface ISettingService
	{
		/// <summary>
		/// Get setting value by key
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="key">Key</param>
		/// <param name="defaultValue">Default value</param>
		/// <returns>Setting value</returns>
		T GetByKey<T>(string key, T defaultValue = default(T));

		/// <summary>
		/// Get setting by key
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns>Setting object</returns>
		SettingViewModel GetByKey(string key);

		/// <summary>
		/// Gets all settings
		/// </summary>
		/// <returns>Setting collection</returns>
		IList<SettingViewModel> GetAll();

		/// <summary>
		/// Gets all settings
		/// </summary>
		/// <returns>Setting collection</returns>
		IDictionary<string, SettingViewModel> GetAllAsDictionary();

		/// <summary>
		/// Load settings
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		T LoadSetting<T>() where T : ISettings, new();
		/// <summary>
		/// Load settings
		/// </summary>
		/// <param name="type">Type</param>
		ISettings LoadSetting(Type type);
	}
	public class SettingService : ISettingService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;
		public SettingService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
		}

		public virtual IDictionary<string, SettingViewModel> GetAllAsDictionary()
		{
			var settings = unitOfWork.Repository<Settings>().GetAll();
			var query = mapper.Map<List<SettingViewModel>>(settings);

			return query.ToDictionary(s => s.Name.ToLowerInvariant());
		}

		public virtual SettingViewModel GetByKey(string key)
		{
			if (String.IsNullOrEmpty(key))
				return null;

			key = key.Trim().ToLowerInvariant();

			var settings = GetAllAsDictionary();
			if (settings.ContainsKey(key))
			{
				var setting = unitOfWork.Repository<Settings>().Get(settings[key].SettingId);
				return mapper.Map<SettingViewModel>(setting);
			}

			return null;
		}

		protected virtual IDictionary<string, IList<Settings>> GetAllSettings()
		{
			//we use no tracking here for performance optimization
			//anyway records are loaded only for read-only operations
			var settings = unitOfWork.Repository<Settings>().GetAll();

			var dictionary = new Dictionary<string, IList<Settings>>();
			foreach (var s in settings)
			{
				var resourceName = s.Name.ToLowerInvariant();
				var settingForCaching = new Settings
				{
					SettingId = s.SettingId,
					Name = s.Name,
					Value = s.Value
				};
				if (!dictionary.ContainsKey(resourceName))
				{
					//first setting
					dictionary.Add(resourceName, new List<Settings>
						{
							settingForCaching
						});
				}
				else
				{
					//already added
					//most probably it's the setting with the same name but for some certain store (storeId > 0)
					dictionary[resourceName].Add(settingForCaching);
				}
			}
			return dictionary;

		}

		public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T),
			int storeId = 0, bool loadSharedValueIfNotFound = false)
		{
			if (string.IsNullOrEmpty(key))
				return defaultValue;

			var settings = GetAllSettings();
			key = key.Trim().ToLowerInvariant();
			if (settings.ContainsKey(key))
			{
				var settingsByKey = settings[key];
				var setting = settingsByKey.FirstOrDefault();

				//load shared value?
				if (setting == null && storeId > 0 && loadSharedValueIfNotFound)
					setting = settingsByKey.FirstOrDefault();

				if (setting != null)
					return CommonHelper.To<T>(setting.Value);
			}

			return defaultValue;
		}
		public virtual T LoadSetting<T>() where T : ISettings, new()
		{
			return (T)LoadSetting(typeof(T));
		}
		public virtual ISettings LoadSetting(Type type)
		{
			var settings = Activator.CreateInstance(type);

			foreach (var prop in type.GetProperties())
			{
				// get properties we can read and write to
				if (!prop.CanRead || !prop.CanWrite)
					continue;

				var key = type.Name + "." + prop.Name;
				//load by store
				var setting = GetSettingByKey<string>(key, loadSharedValueIfNotFound: true);
				if (setting == null)
					continue;

				if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
					continue;

				if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
					continue;

				var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

				//set property
				prop.SetValue(settings, value, null);
			}

			return settings as ISettings;
		}

		public virtual T GetByKey<T>(string key, T defaultValue = default(T))
		{
			if (String.IsNullOrEmpty(key))
				return defaultValue;

			key = key.Trim().ToLowerInvariant();

			var settings = GetAllAsDictionary();
			if (settings.ContainsKey(key))
			{
				var setting = settings[key];
				return setting.As<T>();
			}
			return defaultValue;
		}

		public IList<SettingViewModel> GetAll()
		{
			var settings = unitOfWork.Repository<Settings>().GetAll();
			return mapper.Map<IList<SettingViewModel>>(settings);
		}
	}
}

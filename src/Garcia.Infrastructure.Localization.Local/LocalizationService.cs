using Garcia.Application.Contracts.Localization;
using Garcia.Domain;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Reflection;

namespace Garcia.Infrastructure.Localization.Local
{
    public class LocalizationService : ILocalizationService
    {
        private ILocalizationItemService _localizationItemService;
        private readonly ILogger<LocalizationService> _logger;

        public LocalizationService(ILocalizationItemService localizationItemService,
            ILogger<LocalizationService> logger)
        {
            _localizationItemService = localizationItemService;
            _logger = logger;
        }

        public bool AddMissingItem => true;

        public async Task<string> Localize(string cultureCode, string key)
        {
            var item = await _localizationItemService.GetLocalizationItem(key, cultureCode);

            if (item == null && AddMissingItem)
            {
                await _localizationItemService.AddLocalizationItem(new LocalizationItem(cultureCode, key,
                    string.Empty));
            }

            return item?.Value;
        }

        public async Task Localize(string cultureCode, IId<long> item)
        {
            var type = item.GetType();
            var typeName = type.Name;
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var property in properties)
            {
                var localizableAttribute =
                    Attribute.GetCustomAttribute(property, typeof(LocalizableAttribute)) as LocalizableAttribute;

                if (localizableAttribute != null && localizableAttribute.IsLocalizable)
                {
                    try
                    {
                        var localizedValue = await Localize(cultureCode, $"{typeName}.{item.Id}.{property.Name}");

                        if (!string.IsNullOrEmpty(localizedValue))
                        {
                            property.SetValue(item, localizedValue);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                    }
                }
            }
        }

        public async Task<string> Localize(string cultureCode, Entity<long> item, string propertyName)
        {
            var type = item.GetType();
            var typeName = type.Name;
            var property = type.GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (property == null)
            {
                throw new LocalizationException($"Property {propertyName} does not exist in entity {typeName}.");
            }

            if (property == null)
            {
                throw new LocalizationException($"Property {propertyName} is not a string.");
            }

            var localizedValue = await Localize(cultureCode, $"{typeName}.{item.Id}.{property.Name}");
            return localizedValue;
        }
    }
}
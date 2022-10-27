using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Garcia.Application.Contracts.Localization;
using Garcia.Domain;
using Microsoft.Extensions.Logging;

namespace Garcia.Infrastructure.Localization.Local
{
    public class LocalizationService<T> : ILocalizationService<T>
        where T : ILocalizationItem, new()
    {
        private readonly ILocalizationItemService<T> _localizationItemService;
        private readonly ILogger<LocalizationService<T>> _logger;

        public LocalizationService(ILocalizationItemService<T> localizationItemService,
            ILogger<LocalizationService<T>> logger)
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
                await _localizationItemService.AddLocalizationItem(new T { CultureCode = cultureCode, Key = key, Value = string.Empty });
            }

            return item?.Value;
        }

        public async Task Localize<TKey>(string cultureCode, IEntity<TKey> item)
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

        public async Task<string> Localize<TItem, TKey>(string cultureCode, TItem item, Expression<Func<TItem, object>> expression)
            where TItem : IEntity<TKey>
        {
            var type = typeof(TItem);
            var typeName = type.Name;
            var propertyName = ((MemberExpression)expression.Body).Member.Name;
            var property = type.GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var localizedValue = await Localize(cultureCode, $"{typeName}.{item.Id}.{property!.Name}");
            return localizedValue;
        }
    }
}
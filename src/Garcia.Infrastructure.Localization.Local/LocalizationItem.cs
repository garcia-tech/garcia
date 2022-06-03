using Garcia.Application.Contracts.Localization;
using Garcia.Domain;

namespace Garcia.Infrastructure.Localization.Local
{
    public class LocalizationItem : Entity<long>, ILocalizationItem
    {
        public string Key { get; set; }
        public string CultureCode { get; set; }
        public string Value { get; set; }

        public LocalizationItem()
        {
        }

        public LocalizationItem(string cultureCode, string key, string value)
        {
            Key = key;
            CultureCode = cultureCode;
            Value = value;
        }
    }
}
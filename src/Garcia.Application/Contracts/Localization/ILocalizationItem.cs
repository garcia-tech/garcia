namespace Garcia.Application.Contracts.Localization
{
    public interface ILocalizationItem
    {
        string Key { get; set; }
        string CultureCode { get; set; }
        string Value { get; set; }
    }
}

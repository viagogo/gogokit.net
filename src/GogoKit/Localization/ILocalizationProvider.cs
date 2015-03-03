namespace GogoKit.Localization
{
    public interface ILocalizationProvider
    {
        string LanguageCode { get; }
        string CountryCode { get; }
        string CurrencyCode { get; }
    }
}
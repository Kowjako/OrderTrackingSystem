using OrderTrackingSystem.Logic.DataAccessLayer;

namespace OrderTrackingSystem.Logic.Mappers
{
    internal static class GlobalMapper 
    {
        internal static Localizations MapLocalizationRowToLocalization(LocalizationRow TSource)
        {
            return new Localizations
            {
                Id = TSource.Id,
                City = TSource.Miasto,
                Country = TSource.Kraj,
                Flat = TSource.Mieszkane,
                House = TSource.Budynek,
                Street = TSource.Ulica,
                ZipCode = TSource.Kod
            };
        }
    }
}

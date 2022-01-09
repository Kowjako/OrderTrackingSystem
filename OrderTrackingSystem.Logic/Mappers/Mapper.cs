using OrderTrackingSystem.Logic.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Mappers
{
    public static class Mapper
    {
        public static Localizations ConvertToLocalizations(LocalizationRow row)
        {
            var localization = new Localizations();
            localization.Id = row.Id;
            localization.Country = row.Kraj;
            localization.City = row.Miasto;
            localization.Street = row.Ulica;
            localization.ZipCode = row.Kod;
            localization.House = row.Budynek;
            localization.Flat = row.Mieszkane;
            return localization;
        }
    }
}

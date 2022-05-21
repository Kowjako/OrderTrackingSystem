using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using System;
using OrderTrackingSystem.Logic.Services.Interfaces;

namespace OrderTrackingSystem.Logic.Services
{
    public class LocalizationService : CRUDManager, ILocalizationService
    {
        public async Task<LocalizationDTO> GetLocalizationById(int id)
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from localization in dbContext.Localizations
                            where localization.Id == id
                            select new LocalizationDTO()
                            {
                                Id = localization.Id,
                                Country = localization.Country,
                                City = localization.City,
                                Street = localization.Street,
                                House = localization.House,
                                Apartment = localization.Flat,
                                ZipCode = localization.ZipCode
                            };
                return await query.AsNoTracking().FirstAsync();
            }
        }

        public async Task UpdateLocalization(Localizations localization)
        {
            await base.UpdateEntity(localization);
        }

        public async Task AddNewLocalization(Localizations localization)
        {
            await base.AddEntity(localization);
        }
    }
}

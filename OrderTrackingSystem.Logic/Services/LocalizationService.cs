using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;

namespace OrderTrackingSystem.Logic.Services
{
    public class LocalizationService : IService<Localizations>
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
                                Kraj = localization.Country,
                                Miasto = localization.City,
                                Ulica = localization.Street,
                                Budynek = localization.House,
                                Mieszkanie = localization.Flat,
                                Kod = localization.ZipCode
                            };
                return await query.FirstAsync();
            }
        }

        public async void UpdateLocalization(Localizations localization)
        {
            using(var dbContext = new OrderTrackingSystemEntities())
            {
                dbContext.Entry(localization).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

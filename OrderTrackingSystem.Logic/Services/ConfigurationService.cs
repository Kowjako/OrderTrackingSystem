using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public enum OrderState
    {
        PrepatedBySeller = 0,
        GetFromSeller = 1,
        GetByLocal = 2,
        SentFromLocal = 3,
        ToDelivery = 4,
        ReadyToPickup = 5,
        Getted = 6,
        ComplaintSet = 7,
        ComplaintResolved = 8,
        ReturnToSeller = 9
    }

    public class ConfigurationService : IService<ConfigurationService>
    {
        private static readonly char[] CharArray = 
        {
            'A', 'B', 'C', 'D','E','F','G','H','I','G','K','L'
        };

        public async Task<int?> GetCurrentSessionId()
        {
            var connectionString = @"data source=WLODEKPC\SQLEXPRESS;initial catalog=OrderTrackingSystem;integrated security=True;MultipleActiveResultSets=True";
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand("SELECT AccountId, IsClient FROM Session", sqlConnection))
                {
                    await sqlConnection.OpenAsync();
                    using (var sqlReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while(await sqlReader.ReadAsync())
                        {
                            return sqlReader.GetValue(0) as int?;
                        }
                        return -1;
                    }
                }
            }
        }

        public async Task<List<PickupDTO>> GetPickupPoints()
        {
            using (var dbContext = new OrderTrackingSystemEntities())
            {
                var query = from pickup in dbContext.Pickups
                            let localizationQuery = (from localization in dbContext.Localizations
                                                     where localization.Id == pickup.LocalizationId
                                                     select localization).FirstOrDefault()
                            select new PickupDTO
                            {
                                Id = pickup.Id,
                                Capacity = pickup.Capacity.ToString(),
                                Adres = localizationQuery.Street + " " +
                                        localizationQuery.House,
                                MiastoKod = localizationQuery.City + ", " +
                                            localizationQuery.ZipCode,
                                WorkTime = pickup.WorkHours
                            };
                return await query.ToListAsync();
            }
        }

        public static string GenerateElementNumber()
        {
            var randomizer = new Random();
            return CharArray[randomizer.Next(0, 12)].ToString() +
                   CharArray[randomizer.Next(0, 12)].ToString() +
                   randomizer.Next(100000000, 999999999).ToString();
        }

        public static Tuple<string, string> GetStatusDetails(OrderState state)
        {
            switch(state)
            {
                case OrderState.PrepatedBySeller:
                    return new Tuple<string, string>(Properties.Resources.PrepatedBySeller, Properties.Resources.PrepareBySellerDesc);
                case OrderState.GetFromSeller:
                    return new Tuple<string, string>(Properties.Resources.GetFromSeller, Properties.Resources.GetFromSellerDesc);
                case OrderState.GetByLocal:
                    return new Tuple<string, string>(Properties.Resources.GetByLocal, Properties.Resources.GetByLocalDesc);
                case OrderState.SentFromLocal:
                    return new Tuple<string, string>(Properties.Resources.SentFromLocal, Properties.Resources.SentFromLocalDesc);
                case OrderState.ToDelivery:
                    return new Tuple<string, string>(Properties.Resources.ToDelivery, Properties.Resources.ToDeliveryDesc);
                case OrderState.ReadyToPickup:
                    return new Tuple<string, string>(Properties.Resources.ReadyToPickup, Properties.Resources.ReadyToPickupDesc);
                case OrderState.Getted:
                    return new Tuple<string, string>(Properties.Resources.Getted, Properties.Resources.GettedDesc);
                case OrderState.ComplaintSet:
                    return new Tuple<string, string>(Properties.Resources.ComplaintSet, Properties.Resources.ComplaintSetDesc);
                case OrderState.ComplaintResolved:
                    return new Tuple<string, string>(Properties.Resources.ComplaintResolved, Properties.Resources.ComplaintResolvedDesc);
                case OrderState.ReturnToSeller:
                    return new Tuple<string, string>(Properties.Resources.ReturnToSeller, Properties.Resources.ReturnToSellerDesc);
                default:
                    return null;
            }
        }
    }
}

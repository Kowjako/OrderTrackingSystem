using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace OrderTrackingSystem.Logic.Services
{
    public enum OrderState
    {
        [Display(Name = "PrepatedBySeller", ResourceType = typeof(Properties.Resources))]
        PrepatedBySeller = 0,
        [Display(Name = "GetFromSeller", ResourceType = typeof(Properties.Resources))]
        GetFromSeller = 1,
        [Display(Name = "GetByLocal", ResourceType = typeof(Properties.Resources))]
        GetByLocal = 2,
        [Display(Name = "SentFromLocal", ResourceType = typeof(Properties.Resources))]
        SentFromLocal = 3,
        [Display(Name = "ToDelivery", ResourceType = typeof(Properties.Resources))]
        ToDelivery = 4,
        [Display(Name = "ReadyToPickup", ResourceType = typeof(Properties.Resources))]
        ReadyToPickup = 5,
        [Display(Name = "Getted", ResourceType = typeof(Properties.Resources))]
        Getted = 6,
        [Display(Name = "ComplaintSet", ResourceType = typeof(Properties.Resources))]
        ComplaintSet = 7,
        [Display(Name = "ComplaintResolved", ResourceType = typeof(Properties.Resources))]
        ComplaintResolved = 8,
        [Display(Name = "ReturnToSeller", ResourceType = typeof(Properties.Resources))]
        ReturnToSeller = 9
    }

    public class ConfigurationService : IConfigurationService
    {
        private static readonly char[] CharArray = 
        {
            'A', 'B', 'C', 'D','E','F','G','H','I','G','K','L'
        };

        public async Task<int> GetCurrentSessionId()
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
                            return (int)sqlReader.GetValue(0);
                        }
                        return -1;
                    }
                }
            }
        }

        public async Task<(bool isSuccess, bool accType)> MakeSessionForCredentials(string login, string password)
        {
            var connectionString = @"data source=WLODEKPC\SQLEXPRESS;initial catalog=OrderTrackingSystem;integrated security=True;MultipleActiveResultSets=True";
            (string login, string password, bool accType, int accountId) fetchedData = (string.Empty, string.Empty, false, 0);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand("SELECT Login, Password, AccountType, AccountId FROM Users WHERE login = @login", sqlConnection))
                {
                    var sqlParameter = new SqlParameter("@login", login);
                    sqlCommand.Parameters.Add(sqlParameter);
                    await sqlConnection.OpenAsync();
                    using (var sqlReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await sqlReader.ReadAsync())
                        {
                            fetchedData = (sqlReader.GetValue(0).ToString(), 
                                           sqlReader.GetValue(1).ToString(), 
                                           bool.Parse(sqlReader.GetValue(2).ToString()),
                                           int.Parse(sqlReader.GetValue(3).ToString()));
                        }                       
                    }

                    /* Dane do logowania poprawne zakładamy sesję */
                    if (Cryptography.DecryptFromRSA(fetchedData.password).Equals(password))
                    {
                        using (var sqlCommand1 = new SqlCommand("DELETE FROM Session", sqlConnection))
                        {
                            /* Czyszczenie aktualnej sesji */
                            await sqlCommand1.ExecuteNonQueryAsync();
                        }
                        /* Zakladanie sesji */
                        using (var sqlCommand2 = new SqlCommand("INSERT INTO Session (AccountId, IsClient) VALUES (@AccountId, @IsClient)", sqlConnection))
                        {
                            var sqlParameterAccountId = new SqlParameter("@AccountId", fetchedData.accountId);
                            var sqlParameterIsClient = new SqlParameter("@IsClient", fetchedData.accType);
                            sqlCommand2.Parameters.Add(sqlParameterAccountId);
                            sqlCommand2.Parameters.Add(sqlParameterIsClient);
                            await sqlCommand2.ExecuteNonQueryAsync();
                            return (true, fetchedData.accType);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid credentials");
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
                                Address = localizationQuery.Street + " " +
                                        localizationQuery.House,
                                CityWithCode = localizationQuery.City + ", " +
                                            localizationQuery.ZipCode,
                                WorkTime = pickup.WorkHours
                            };
                return await query.ToListAsync();
            }
        }

        public string GenerateElementNumber()
        {
            var randomizer = new Random();
            return CharArray[randomizer.Next(0, 12)].ToString() +
                   CharArray[randomizer.Next(0, 12)].ToString() +
                   randomizer.Next(100000000, 999999999).ToString();
        }

        /* Using new tuple */
        public (string name, string description) GetStatusDetails(OrderState state)
        {
            /* Switch expression C# 8.0 - mozna bylo i od razu => state switch*/
            return state switch
            {
                OrderState.PrepatedBySeller => (Properties.Resources.PrepatedBySeller, Properties.Resources.PrepareBySellerDesc),
                OrderState.GetFromSeller => (Properties.Resources.GetFromSeller, Properties.Resources.GetFromSellerDesc),
                OrderState.GetByLocal => (Properties.Resources.GetByLocal, Properties.Resources.GetByLocalDesc),
                OrderState.SentFromLocal => (Properties.Resources.SentFromLocal, Properties.Resources.SentFromLocalDesc),
                OrderState.ToDelivery => (Properties.Resources.ToDelivery, Properties.Resources.ToDeliveryDesc),
                OrderState.ReadyToPickup => (Properties.Resources.ReadyToPickup, Properties.Resources.ReadyToPickupDesc),
                OrderState.Getted => (Properties.Resources.Getted, Properties.Resources.GettedDesc),
                OrderState.ComplaintSet => (Properties.Resources.ComplaintSet, Properties.Resources.ComplaintSetDesc),
                OrderState.ComplaintResolved => (Properties.Resources.ComplaintResolved, Properties.Resources.ComplaintResolvedDesc),
                OrderState.ReturnToSeller => (Properties.Resources.ReturnToSeller, Properties.Resources.ReturnToSellerDesc),
                _ => (string.Empty, string.Empty)
            };
        }

        public IEnumerable<Tuple<string, OrderState>> GetAllStates()
        {
            foreach(var state in Enum.GetValues(typeof(OrderState)))
            {
                yield return new Tuple<string, OrderState>(EnumConverter.GetNameByIdLocalized<OrderState>((int)state), (OrderState)state);
            }
        }

        public async Task<List<ProcessDTO>> GetAutoProcesses()
        {
            using var context = new OrderTrackingSystemEntities();
            var query = from process in context.Processes
                        select new ProcessDTO
                        {
                            Name = process.Name,
                            LastProcessDate = process.LastProcessDate,
                            Description = process.Description,
                            StoredProcedureFunction = process.StoredProcedureName
                        };
            return await query.ToListAsync();
        }

        public async Task AddNewSellerProcess(ProcessDTO NewSellerProcess, string _sqlProcessScript)
        {
            using (var transactionScope = D3TransactionScope.GetTransactionScope())
            {
                var connectionString = @"data source=WLODEKPC\SQLEXPRESS;initial catalog=OrderTrackingSystem;integrated security=True;MultipleActiveResultSets=True";
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    await sqlConnection.OpenAsync();

                    // Dodanie konfiguracji procesu
                    using (var sqlCommand = new SqlCommand("INSERT INTO Processes (Name, LastProcessDate, Description, StoredProcedureName) VALUES (@Name, @Date, @Descr, @ProcName)"))
                    {
                        sqlCommand.Connection = sqlConnection;
                        var nameParameter = new SqlParameter("@Name", NewSellerProcess.Name);

                        var dateParameter = new SqlParameter("@Date", SqlDbType.SmallDateTime);
                        dateParameter.Value = DBNull.Value;

                        var descrParameter = new SqlParameter(@"Descr", NewSellerProcess.Description);
                        var procNameParameter = new SqlParameter("@ProcName", NewSellerProcess.StoredProcedureFunction);

                        sqlCommand.Parameters.AddRange(new[] { nameParameter, dateParameter, descrParameter, procNameParameter });

                        await sqlCommand.ExecuteNonQueryAsync();
                    }

                    string[] commands = Regex.Split(_sqlProcessScript, @"\bGO\b");

                    //Dodanie procedury do bazy
                    await commands.ToList().ForEachAsync(async s =>
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            using (var sqlCommand = new SqlCommand(s, sqlConnection))
                            {
                                await sqlCommand.ExecuteNonQueryAsync();
                            }
                        }
                    });
                }

                transactionScope.Complete();
            }
        }
    }
}

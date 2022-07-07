using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [CollectionDefinition("DBCollection")]
    public class ServiceDBCollectionFixture : ICollectionFixture<DatabaseFixture> { }


    //fixture class for database
    public class DatabaseFixture : IDisposable
    {
        public SqlConnection ConnectionDB { get; private set; }

        public DatabaseFixture()
        {
            ConnectionDB = new SqlConnection();
            ConnectionDB.Open();
        }

        public void Dispose()
        {
            //usun dane testowe i zamknij polaczenie
            ConnectionDB.Close();
        }
    }
}

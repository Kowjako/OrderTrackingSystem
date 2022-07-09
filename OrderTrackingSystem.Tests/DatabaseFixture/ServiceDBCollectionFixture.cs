using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderTrackingSystem.Tests.DatabaseFixture
{
    [CollectionDefinition("DBCollection", DisableParallelization = true)]
    public class ServiceDBCollectionFixture : ICollectionFixture<DBFixture> { }

    public class DBFixture : IDisposable
    {
        public SqlConnection ConnectionDB { get; private set; }
        public bool ShouldDropTestDB { get; set; } = false;

        public DBFixture()
        {
            ConnectionDB = new SqlConnection(@"data source=WLODEKPC\SQLEXPRESS;integrated security=True;MultipleActiveResultSets=False;App=OrderTrackingSystemAPI");
            ConnectionDB.Open();
            TestDatabaseCreator.CreateTestLocalDB(ConnectionDB);
        }

        public void Dispose()
        {
            if(ShouldDropTestDB)
            {
                TestDatabaseCreator.DropTestLocalDB();
            }
            ConnectionDB.Close();
        }
    }
}

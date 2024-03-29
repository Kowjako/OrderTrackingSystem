﻿using OrderTrackingSystem.Logic.Services;
using System;
using System.Data.SqlClient;
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
            ConnectionDB = new SqlConnection(ConfigurationService.D3ConnectionStringMarsOff);
            ConnectionDB.Open();
            TestDatabaseCreator.CreateTestLocalDB(ConnectionDB);
        }

        public void Dispose()
        {
            if (ShouldDropTestDB)
            {
                TestDatabaseCreator.DropTestLocalDB();
                ConnectionDB.Close();
            }
        }
    }
}

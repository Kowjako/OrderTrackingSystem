using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace OrderTrackingSystem.Tests.DatabaseFixture
{
    public class TestDatabaseCreator
    {
        private static SqlConnection dbConntection;
        private static StringBuilder query = new StringBuilder();
        public static void CreateTestLocalDB(SqlConnection db)
        {
            dbConntection = db;

            using (var sr = new StreamReader(@"../../SQLTestDB/SetupTestLocalDB.sql"))
            {
                query.Append(sr.ReadToEnd());
            }
            var scriptPack = Regex.Split(query.ToString(), @"\bGO\b");
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = dbConntection;
                foreach (var script in scriptPack)
                {
                    if (!string.IsNullOrEmpty(script))
                    {
                        sqlCommand.CommandText = script;
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void DropTestLocalDB()
        {
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = dbConntection;
                sqlCommand.CommandText = "USE master";
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = "DROP DATABASE OrderTrackingSystem_Test";
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}

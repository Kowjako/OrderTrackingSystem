using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.HelperClasses
{
    public static class ProcessRunner
    {
        private static SqlConnection connection = null;

        private static async Task OpenConnection()
        {
            connection = new SqlConnection();
            await connection.OpenAsync();
        }

        private static void CloseConnection()
        {
            connection?.Close();
        }

        public static async Task RunProcedure(string procedureName)
        {
            try
            {
                await OpenConnection();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    var sqlCommand = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = "EXEC ProcessRunner_v2 @procedureName"
                    };

                    var sqlParam = new SqlParameter("@procedureName", procedureName);
                    sqlCommand.Parameters.Add(sqlParam);

                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch { throw; }
            finally
            {
                CloseConnection();
            }
        }
    }
}
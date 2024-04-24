namespace app.MVVM.Model.Data.SqlCommandHandlers
{
    using System.Diagnostics;
    using app.MVVM.Model.Data.ServerHandlers;
    using Microsoft.Data.SqlClient;

    public interface ISqlCommandExecutor
    {
        bool ExecuteNonQueryCommandFromString(string query);
    }

    public class SqlCommandExecutor : ISqlCommandExecutor
    {
        private readonly SqlConnection currentSqlConnection;

        public SqlCommandExecutor()
        {
            currentSqlConnection = StaticSqlConnectionGenerator.GetConnection();
        }

        public bool ExecuteNonQueryCommandFromString(string query)
        {
            try
            {
                currentSqlConnection.Open();
                SqlCommand command = new SqlCommand(query, currentSqlConnection);
                command.ExecuteNonQuery();
                currentSqlConnection.Close();
                return true;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                return false;
            }
        }

    }

}

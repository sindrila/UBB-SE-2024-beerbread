namespace app.MVVM.Model.Data.ServerHandlers
{
    using Microsoft.Data.SqlClient;

    public class StaticSqlConnectionGenerator
    {
        private static SqlConnection? sqlConnection;

        public static SqlConnection GetConnection()
        {
            string connectionString = GetConnectionString();
            sqlConnection ??= new SqlConnection(connectionString);
            return sqlConnection;
        }

        public static string GetConnectionString()
        {
            // return "Data Source=DESKTOP-3O5NAR2\\SQLEXPRESS;Initial Catalog=breadcrumbwhatever;Persist Security Info=True;User ID=sa;Password=iss;Trust Server Certificate=True";
            return "Data Source=ALEXBEC2\\SQLEXPRESS;Initial Catalog=breadcrumbwhatever;Persist Security Info=True;User ID=sa;Password=iss;Encrypt=True;Trust Server Certificate=True";
        }
    }
}

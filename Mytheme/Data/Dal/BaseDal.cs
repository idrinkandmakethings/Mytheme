using System.Data.SQLite;


namespace Mytheme.Data.Dal
{
    public class BaseDal
    {
        protected readonly string connectionString;

        public BaseDal(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

    }
}

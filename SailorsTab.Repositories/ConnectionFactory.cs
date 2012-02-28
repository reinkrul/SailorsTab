using System;
using System.Data;
using System.Data.SQLite;

namespace SailorsTab.Repositories
{
	public class ConnectionFactory
	{
		public IDbConnection createConnection(string connectionString)
		{
            IDbConnection connection = new SQLiteConnection(connectionString);
            connection.Open();

            // Make sure changes are written directly to the database
            // (has a lower chance of corrupting the database on a crash or power failure
            // but is slower than 'NORMAL').
            // http://www.sqlite.org/pragma.html#pragma_synchronous
            IDbCommand command = connection.CreateCommand();
            command.CommandText = "PRAGMA synchronous = FULL;";
            command.ExecuteNonQuery();

            return connection;
		}
	}
}


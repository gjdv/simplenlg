using System;
using System.Data.SQLite;

namespace SimpleNLG.Main.lexicon
{
    public class SqliteAccess
    {
        public static void ProcessDataReader(SQLiteConnection connection, string sql, Action<SQLiteDataReader> action)
        {
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            ProcessDataReader(connection, sql, action);
            command.Dispose();
        }

        public static void ProcessDataReader(SQLiteConnection connection, SQLiteCommand command, Action<SQLiteDataReader> action)
        { 
            connection.Open();

            try
            {

                try
                {
                    SQLiteDataReader reader = command.ExecuteReader();

                    try
                    {

                        while (reader.Read())
                        {
                            action?.Invoke(reader);
                        }
                    }
                    finally
                    {
                        reader.  Close();
                        reader.Dispose();
                    }
                }
                finally
                {
                    command.Dispose();
                }
            }
            finally
            {
                connection.Close();
            }

        }
    }
}

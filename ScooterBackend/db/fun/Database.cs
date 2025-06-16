using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ScooterBackend{
    public static class DatabaseFunctions{

        public static List<string> Query(string queryString){
            using(var connection = new SqliteConnection("Data Source=db/db/INFSQScooterBackend.db")){
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                queryString;


                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    List<string> result = new();
                    while (reader.Read())
                    {
                        result.Append(reader.GetString(0));
                    }
                    return result;
                }
            }
        }
    }
}

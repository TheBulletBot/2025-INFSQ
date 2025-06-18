using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ScooterBackend{
    public static class DatabaseHelper{

        public static List<T> Query<T>(string queryString){
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
                    List<T> the = new();
                    foreach (var json in result)
                    {
                        try
                        {
                            T obj = JsonSerializer.Deserialize<T>(json);
                            if (obj != null)
                            {
                                the.Add(obj);
                            }
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Failed to deserialize: {ex.Message}");
                        }
                    }
                    return the;
                }
            }
        }
    }
}

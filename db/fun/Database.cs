using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


public static class DatabaseHelper
{
    public static string DatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"../../../db/db/INFSQScooterBackend.db");
    private static string readString = new SqliteConnectionStringBuilder()
    {
        Mode = SqliteOpenMode.ReadOnly,
        DataSource = DatabaseHelper.DatabasePath
    }.ToString();
    
    private static string modifyDBConnectionString = new SqliteConnectionStringBuilder()
        {
            Mode = SqliteOpenMode.ReadWriteCreate,
            DataSource = DatabaseHelper.DatabasePath
        }.ToString();

    /// <summary>
    /// DO NOT USE THIS VERSION OF THE FUNCTION IF YOU CAN AVOID IT. THIS IS UNSAFE. USE THE ONE THAT TAKES SQLITECOMMAND INSTEAD.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public static List<T> Query<T>(string query)
    {
        var queryCommand = new SqliteCommand(query);
        return Query<T>(queryCommand);
    }

    /// <summary>
    /// Performs a Query to the Database that automatically enters into JSON C# object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public static List<T> Query<T>(SqliteCommand query)
    {
        using (var connection = new SqliteConnection(readString))
        {
            connection.Open();

            query.Connection = connection;

            using (SqliteDataReader reader = query.ExecuteReader())
            {
                List<string> result = new();
                while (reader.Read())
                {

                    var row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        row[columnName] = value;
                    }

                    string json = JsonSerializer.Serialize(row);
                    result.Add(json);
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
    public static List<string> QueryAsString(string queryString) {
        using (var connection = new SqliteConnection(readString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
            queryString;


            using (SqliteDataReader reader = command.ExecuteReader())
            {
                List<string> result = new();
                while (reader.Read())
                {
                    result.Add(reader.GetString(0));
                }
                return result;
            }
        }
    }
    public static void ExecuteStatement(string queryString)
    {
        using (var connection = new SqliteConnection(modifyDBConnectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Executed Statement");
        }
    }
}


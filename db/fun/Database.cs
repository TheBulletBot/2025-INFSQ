using System.Collections.Generic;
using System.Data.SQLite;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;


public static class DatabaseHelper
{
    public static string DatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../db/db/INFSQScooterBackend.db");
    private static string readString = new SQLiteConnectionStringBuilder()
    {
        ReadOnly = true,
        DataSource = DatabaseHelper.DatabasePath
    }.ToString();

    private static string modifyDBConnectionString = new SQLiteConnectionStringBuilder()
    {
        ReadOnly = false,
        DataSource = DatabaseHelper.DatabasePath
    }.ToString();

    /// <summary>
    /// DO NOT USE THIS VERSION OF THE FUNCTION IF YOU CAN AVOID IT. THIS IS UNSAFE. USE THE ONE THAT TAKES SQLiteCOMMAND INSTEAD.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public static List<T> Query<T>(string query)
    {
        var queryCommand = new SQLiteCommand(query);
        return Query<T>(queryCommand);
    }

    /// <summary>
    /// Performs a Query to the Database that automatically enters into JSON C# object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <returns></returns>
    public static List<T> Query<T>(SQLiteCommand query)
    {
        using (var connection = new SQLiteConnection(readString))
        {
            connection.Open();

            query.Connection = connection;

            using (SQLiteDataReader reader = query.ExecuteReader())
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
    public static List<string> QueryAsString(string queryString)
    {
        using (var connection = new SQLiteConnection(readString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = queryString;

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                List<string> result = new();
                while (reader.Read())
                {
                    var value = reader.IsDBNull(0) ? "" : reader.GetValue(0).ToString();
                    result.Add(value);
                }
                return result;
            }
        }
    }

    public static List<string> QueryAsString(SQLiteCommand cmd)
    {
        using (var connection = new SQLiteConnection(readString))
        {
            connection.Open();
            cmd.Connection = connection;

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                List<string> result = new();
                while (reader.Read())
                {
                    var the = reader.GetString(0);
                    result.Add(the);
                }
                return result;
            }
        }
    }
    public static void ExecuteStatement(string queryString)
    {
        using (var connection = new SQLiteConnection(modifyDBConnectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                command.CommandType = System.Data.CommandType.Text;
                command.Connection = connection;
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Executed Statement");
        }
    }

    public static void ExecuteStatement(SQLiteCommand query)
    {
        using (var connection = new SQLiteConnection(modifyDBConnectionString))
        {
            connection.Open();
            query.Connection = connection;

            query.ExecuteNonQuery();

            Console.WriteLine("Executed Statement");
        }
    }
}


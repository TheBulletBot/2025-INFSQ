using Microsoft.Data.Sqlite;
using ScooterBackend;
using System;
using System.IO;

public static class DBSetup
{
    private static string ConnectionString = new SqliteConnectionStringBuilder()
    {
        Mode = SqliteOpenMode.ReadWriteCreate,
        DataSource = Path.Combine("db", "db", "INFSQScooterBackend.db")
    }.ToString();

    public static void SetupDB()
    {
        string folder = Path.Combine("db", "db");
        string dbPath = Path.Combine(folder, "INFSQScooterBackend.db");

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
            Console.WriteLine("📁 Map aangemaakt: " + folder);
        }

        if (!File.Exists(dbPath))
        {
            File.Create(dbPath).Close();
            Console.WriteLine("✅ Databasebestand aangemaakt: " + dbPath);
        }

        InitScooterTable();
        InitTravelerTable();
        PopulateScooterTable();
        // PopulateTravelerTable(); // Optioneel
    }

    private static void InitScooterTable()
    {
        string queryString = @"CREATE TABLE IF NOT EXISTS Scooter(
            SerialNumber INTEGER PRIMARY KEY AUTOINCREMENT,
            Brand TEXT,
            Model TEXT,
            TopSpeed INTEGER,
            BatteryCapacity INTEGER,
            StateOfCharge INTEGER,
            TargetMin INTEGER,
            TargetMax INTEGER,
            Mileage INTEGER,
            Location TEXT,
            OutOfService INTEGER,
            LastService DATE
        );";

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = queryString;
        command.ExecuteNonQuery();

        Console.WriteLine("✅ Scooter-tabel aangemaakt.");
    }

    private static void InitTravelerTable()
    {
        string query = @"CREATE TABLE IF NOT EXISTS Traveller(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT UNIQUE,
            PasswordHash TEXT,
            FirstName TEXT,
            LastName TEXT,
            Birthday TEXT,
            Gender TEXT, 
            Street TEXT,
            HouseNumber TEXT,
            ZipCode TEXT,
            City TEXT,
            Mail TEXT,
            Phone TEXT,
            LicenseNumber TEXT UNIQUE NOT NULL,
            RegistrationDate TEXT
        );";

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();

        Console.WriteLine("✅ Traveller-tabel aangemaakt.");
    }

    public static void PopulateScooterTable()
    {
        string query = @"INSERT INTO Scooter (
                Brand, Model, TopSpeed, BatteryCapacity, StateOfCharge,
                TargetMin, TargetMax, Mileage, Location, OutOfService, LastService
            ) VALUES 
                ('brandname', 'modelname', 30, 100, 70, 30, 80, 3456, 'here', 0, '2025-06-01'),
                ('Hyundai', 'R14', 30, 100, 80, 30, 80, 1295, '19.94636:162.84792', 0, '2025-06-01'),
                ('Hyundai', 'R14', 30, 100, 40, 30, 80, 8465, '19.94636:162.84792', 0, '2025-06-01');";

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = query;
        int inserted = command.ExecuteNonQuery();

        Console.WriteLine($"✅ {inserted} scooters toegevoegd.");
    }

    private static void PopulateTravelerTable()
    {
        string query = @"INSERT INTO Traveller (
            Username, PasswordHash, FirstName, LastName, Birthday, Gender,
            Street, HouseNumber, ZipCode, City, Mail, Phone, LicenseNumber, RegistrationDate
        ) VALUES (
            'FunnyWordMan', 'hashedpass123', 'Kevin', 'Kranendonk', '2001-12-10', 'male',
            'Wijnhaven', '107', '0000AA', 'Rotterdam', 'test@mail.com', '33445566', 'AB1234567', '2020-01-01'
        );";

        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.ExecuteNonQuery();

        Console.WriteLine("✅ Traveller testdata toegevoegd.");
    }
}

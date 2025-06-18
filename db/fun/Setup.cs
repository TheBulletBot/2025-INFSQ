using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using SQLitePCL;
public static class DBSetup
{
    private static string ConnectionString = new SqliteConnectionStringBuilder()
    {
        Mode = SqliteOpenMode.ReadWriteCreate,
        DataSource = "INFSQScooterBackend.db"
    }.ToString();
    public static void SetupDB()
    {
        if (!File.Exists("db/db/INFSQScooterBackend.db"))
        {
            CreateDBFile();
        }
        if (IsDatabaseEmpty())
        {
            InitScooterTable();
            PopulateScooterTable();
            InitTravelerTable();
            PopulateTravelerTable();
            InitUserTable();
            //PopulateUserTable();
        }
    }
    private static void CreateDBFile()
    {
        File.Create("db/db/INFSQScooterBackend.db");
    }
    private static bool IsDatabaseEmpty()
    {
        var scooterContents = DatabaseHelper.QueryAsString("SELECT * FROM Scooter");
        var userContents = DatabaseHelper.QueryAsString("SELECT * FROM User");
        var travelerContents = DatabaseHelper.QueryAsString("SELECT * FROM Traveler");
        if (scooterContents.Count == 0 || userContents.Count == 0 || travelerContents.Count == 0)
            return true;
        else
        {
            return false;
        }
    }
    private static void InitScooterTable()
    {
        string queryString = @"CREATE TABLE IF NOT EXISTS Scooter(
            SerialNumber TEXT AUTO INCREMENT PRIMARY KEY NOT NULL UNIQUE, 
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
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Table created successfully.");
        }
    }
    public static void PopulateScooterTable()
    {
        string queryString = @"INSERT INTO Scooter(
                    SerialNumber,
                    Brand,
                    Model,
                    TopSpeed,
                    BatteryCapacity,
                    StateOfCharge,
                    TargetMin,
                    TargetMax,
                    Mileage,
                    Location,
                    OutOfService,
                    LastService) 
                    
                    VALUES(
                    2,
                    'brandname',
                    'modelname',
                    30,
                    100,
                    70,
                    30,
                    80,
                    3456,
                    'here',
                    0,
                    1762025),
                    (3,
                    'Hyundai',
                    'R14',
                    30,
                    100,
                    80,
                    30,
                    80,
                    1295,
                    '19.94636:162.84792',
                    0,
                    1-6-2025),
                    (4,
                    'Hyundai',
                    'R14',
                    30,
                    100,
                    40,
                    30,
                    80,
                    8465,
                    '19.94636:162.84792',
                    0,
                    1-6-2025);";

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = queryString;
                var me = command.ExecuteNonQuery();
                System.Console.WriteLine(me);
            }

            Console.WriteLine("Inserted Seed data into Scooter.");
        }
    }
    private static void InitTravelerTable()
    {
        DatabaseHelper.ExecuteStatement(@"CREATE TABLE IF NOT EXISTS Traveller(
            Id INT AUTO INCREMENT PRIMARY KEY NOT NULL UNIQUE,
            Username VARCHAR(10) UNIQUE,
            PasswordHash INT,
            FirstName VARCHAR(255),
            LastName VARCHAR(255),
            Birthday VARCHAR(255),
            Gender VARCHAR(255), 
            Street VARCHAR(255),
            HouseNumber VARCHAR(5),
            ZipCode VARCHAR(255),
            City VARCHAR(255),
            Mail VARCHAR(255),
            Phone VARCHAR (8),
            LicenseNumber TEXT UNIQUE NOT NULL,
            RegistrationDate TEXT
            );");

        System.Console.WriteLine("Successfully Created Table");

    }
    private static void PopulateTravelerTable()
    {
        //Don't Forget to Encrypt Usernames, names, and Addresses later. 
        DatabaseHelper.ExecuteStatement(@"
            INSERT INTO Traveler(Id, Username, PasswordHash, FirstName, LastName, Birthday, Gender, Street, HouseNumber, ZipCode, City, Main, Phone, LicenseNumber)
            VALUES(1,'FunnyWordMan',15637621463,'kevin','Kranendonk','10-12-2001','male','Wijnhaven','107','0000AA','Rotterdam','33445566','7863476537683324','1-1-2020')
        ");
        Console.WriteLine("Inserted Seed data into Traveler.");
    }
    private static void InitUserTable()
    {
        DatabaseHelper.ExecuteStatement(@"CREATE TABLE IF NOT EXISTS 
        User(
        Id INT AUTO INCREMENT PRIMARY KEY NOT NULL UNIQUE,
        Username VARCHAR(10) UNIQUE,
        PasswordHash INT,
        Role TEXT, 
        FirstName TEXT, 
        LastName TEXT,
        registrationDate TEXT);
        ");
    }
}
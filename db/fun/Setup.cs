using System.Data.SQLite;
/// <summary>
/// This class and these functions are here to quickly set up a database when one doesn't exist. 
/// THIS IS FOR THE GRADING TEACHER'S CONVENIENCE. So that they have access to at lease SOME sample data when the database starts.
/// </summary>
public static class DBSetup
{
    private static string ConnectionString = new SQLiteConnectionStringBuilder()
    {
        ReadOnly = false,
        DataSource = DatabaseHelper.DatabasePath
    }.ToString();
    public static void SetupDB()
    {
        if (!File.Exists(DatabaseHelper.DatabasePath))
        {
            CreateDBFile();
            InitScooterTable();
            InitTravelerTable();
            InitUserTable();
            InitDBBackupTable();
        }
        if (IsDatabaseEmpty())
        {
            InitScooterTable();
            PopulateScooterTable();
            InitTravelerTable();
            PopulateTravelerTable();
            InitUserTable();
            PopulateUserTable();
        }
    }
    private static void CreateDBFile()
    {
        File.WriteAllText(DatabaseHelper.DatabasePath, "");
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
            SerialNumber TEXT PRIMARY KEY NOT NULL UNIQUE, 
            Brand TEXT,
            Model TEXT,
            TopSpeed INTEGER,
            BatteryCapacity INTEGER,
            StateOfCharge INTEGER,
            TargetRange TEXT,
            Mileage INTEGER,
            Location TEXT,
            OutOfService INTEGER,
            LastService DATE
            );";
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine( e);
            }
            

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
                    TargetRange,
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
                    '30-80',
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
                    '30-80',
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
                    '30-80',
                    8465,
                    '19.94636:162.84792',
                    0,
                    1-6-2025);";

        using (var connection = new SQLiteConnection(ConnectionString))
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
        DatabaseHelper.ExecuteStatement(@"CREATE TABLE IF NOT EXISTS Traveler(
            Id TEXT PRIMARY KEY NOT NULL UNIQUE,
            Username TEXT UNIQUE,
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
            );");

        System.Console.WriteLine("Successfully Created Table");

    }
    private static void PopulateTravelerTable()
    {
        var encryptedStreet = CryptographyHelper.Encrypt("Wijnhaven");
        var encryptedHN = CryptographyHelper.Encrypt("107");
        var encryptedCity = CryptographyHelper.Encrypt("Rotterdam");
        var encryptedZip = CryptographyHelper.Encrypt("0000AA");
        var encryptedPhone = CryptographyHelper.Encrypt("33445566");
        var encryptedLicense = CryptographyHelper.Encrypt("7863476537683324");
        //Don't Forget to Encrypt Usernames, names, and Addresses later. 
        DatabaseHelper.ExecuteStatement($@"
            INSERT INTO Traveler(Id, 
            FirstName, 
            LastName, 
            Birthday, 
            Gender, 
            Street, 
            HouseNumber, 
            ZipCode, 
            City, 
            Mail, 
            Phone, 
            LicenseNumber)

            VALUES(1,
            'kevin',
            'Kranendonk',
            '10-12-2001',
            'male',
            '{encryptedStreet}',
            '{encryptedHN}',
            '{encryptedZip}',
            '{encryptedCity}',
            '{encryptedPhone}',
            '{encryptedLicense}',
            '1-1-2020')
        ");
        Console.WriteLine("Inserted Seed data into Traveler.");
    }
    private static void InitUserTable()
    {
        DatabaseHelper.ExecuteStatement(@"CREATE TABLE IF NOT EXISTS 
        User(
        Id TEXT PRIMARY KEY NOT NULL UNIQUE,
        Username TEXT UNIQUE,
        PasswordHash TEXT,
        Role TEXT, 
        FirstName TEXT, 
        LastName TEXT,
        RegistrationDate TEXT);
        ");
        Console.WriteLine("Created User Table.");
    }
    private static void PopulateUserTable()
    {
        var encryptedUsername1 = CryptographyHelper.Encrypt("FunnyWordMan");
        var hashedPassword = CryptographyHelper.CreateHashValue("bepsi");
        DatabaseHelper.ExecuteStatement($@"
        INSERT INTO User(Id,Username, PasswordHash,Role,FirstName,LastName,RegistrationDate)
        VALUES(1,'{encryptedUsername1}','{hashedPassword}','ADMIN','Moo','Snuckle','18-06-2025')
        ");
        Console.WriteLine("Inserted Seed data into User.");
    }

    private static void InitDBBackupTable()
    {
        DatabaseHelper.ExecuteStatement(@"CREATE TABLE IF NOT EXISTS 
        DBBackup(
        AdminId TEXT NOT NULL,
        BackupCode TEXT NOT NULL,
        DbPath TEXT NOT NUll
        );
        ");
        Console.WriteLine("Created User Table.");
    }
}
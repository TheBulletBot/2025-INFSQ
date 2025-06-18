using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ScooterBackend;


public class SystemADFunc
{
    private readonly byte[] _aesKey = Encoding.UTF8.GetBytes("1234567890ABCDEF");
    private readonly byte[] _aesIV = Encoding.UTF8.GetBytes("FEDCBA0987654321");
    private readonly string connection = "Data Source=C:\\Users\\rensg\\OneDrive\\Documenten\\GitHub\\2025-INFSQ-1\\ScooterBackend\\db\\fun\\INFSQScooterBackend.db";

    public void AddEngineer(string username, string password)
    {
        string passwordHash = HashPassword(password);

        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();

            string sql = @"INSERT INTO User (Id, Username, PasswordHash, Role)
               VALUES (@Id, @Username, @PasswordHash, @Role)";



            using (var cmd = new SQLiteCommand(sql, conn))
            {
               cmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@Role", "Service Engineer");

                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Service Engineer toegevoegd!");
    }

    public void UpdateEngineer(string currentUsername, string newUsername, string newPassword)
    {
        string passwordHash = HashPassword(newPassword);

        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();
            string sql = @"UPDATE User 
                        SET Username = @NewUsername, 
                            PasswordHash = @PasswordHash 
                        WHERE Username = @CurrentUsername AND Role = 'Service Engineer'";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@CurrentUsername", currentUsername);
                cmd.Parameters.AddWithValue("@NewUsername", newUsername);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    Console.WriteLine(" Service Engineer bijgewerkt.");
                else
                    Console.WriteLine(" Gebruiker niet gevonden of geen Service Engineer.");
            }
        }
    }


    public void DeleteEngineer(string username)
    {
        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();
            string sql = "DELETE FROM USER WHERE Username = @Username AND Role = 'Service Engineer'";
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Service Engineer verwijderd: " + username);
    }

    public void ResetEngineerPassword(string username)
    {
        string tempPassword = "Temp" + new Random().Next(1000, 9999);
        string passwordHash = HashPassword(tempPassword);

        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();
            string sql = @"UPDATE User 
                        SET PasswordHash = @PasswordHash 
                        WHERE Username = @Username AND Role = 'Service Engineer'";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    Console.WriteLine($" Tijdelijk wachtwoord ingesteld voor {username}: {tempPassword}");
                else
                    Console.WriteLine(" Gebruiker niet gevonden of is geen Service Engineer.");
            }
        }
    }


    public void UpdateOwnPassword(string username, string newPassword)
    {

        // kan nu nog alle ww veranderen
        string passwordHash = HashPassword(newPassword);

        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();
            string sql = "UPDATE User SET PasswordHash = @PasswordHash WHERE Username = @Username";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine(" Je account is bijgewerkt.");
    }

        public void DeleteOwnAccount(string username)
        {

            // kan nu alle accounts nog verwijderen 
            using (var conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string sql = "DELETE FROM User WHERE Username = @Username";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine(" Je account is verwijderd.");
        }

    public void SearchAndPrintTravellers(string searchKey)
    {
        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();
            string sql = @"SELECT * FROM Traveller
                        WHERE FirstName LIKE @Search
                            OR LastName LIKE @Search
                            OR Phone LIKE @Search
                            OR Email LIKE @Search
                            OR LicenseNumber LIKE @Search";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Search", "%" + searchKey + "%");

                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\n--- Zoekresultaten ---\n");

                    bool found = false;
                    while (reader.Read())
                    {
                        found = true;
                        Console.WriteLine($"ID: {reader["Id"]}");
                        Console.WriteLine($"Naam: {reader["FirstName"]} {reader["LastName"]}");
                        Console.WriteLine($"Geboortedatum: {reader["Birthday"]}");
                        Console.WriteLine($"Geslacht: {reader["Gender"]}");
                        Console.WriteLine($"Adres: {reader["StreetName"]} {reader["HouseNumber"]}, {reader["ZipCode"]} {reader["City"]}");
                        Console.WriteLine($"Email: {reader["Email"]}");
                        Console.WriteLine($"Telefoon: {reader["Phone"]}");
                        Console.WriteLine($"Rijbewijs: {reader["LicenseNumber"]}");
                        Console.WriteLine("--------------------------\n");
                    }

                    if (!found)
                    {
                        Console.WriteLine("⚠️ Geen reizigers gevonden met die zoekterm.");
                    }
                }
            }
        }
    }



    public void ShowAllUsersAndRoles()
    {

        string query = "SELECT Username, Role FROM User ORDER BY Username ASC";
        var users = DatabaseHelper.Query<User>(query);

        Console.WriteLine("Gebruikers en rollen:\n");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Username} - {user.Role}");
        }
    }
    public void DeleteTraveller(string firstName, string lastName, string phoneNumber)
    {
        string encryptedPhone = Encrypt(phoneNumber);

        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();

            string sql = @"DELETE FROM Traveller
                        WHERE FirstName = @FirstName 
                            AND LastName = @LastName 
                            AND Phone = @Phone";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Phone", encryptedPhone);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    Console.WriteLine($" Traveller verwijderd: {firstName} {lastName}");
                else
                    Console.WriteLine(" Geen traveller gevonden met die gegevens.");
            }
        }
    }

    public void AddScooter(string brand, string model, int topSpeed, int battery, int charge, int totalRange, string location, int outOfService, int mileage, DateTime lastMaintenance)
    {
        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();
            string sql = @"INSERT INTO Scooter 
                          (Brand, Model, TopSpeed, BatteryCapacity, StateOfCharge, TargetRange, Location, OutOfService, Mileage, LastMaintenance)
                          VALUES
                          (@Brand, @Model, @TopSpeed, @BatteryCapacity, @StateOfCharge, @TargetRange, @Location, @OutOfService, @Mileage, @LastMaintenance)";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Brand", brand);
                cmd.Parameters.AddWithValue("@Model", model);
                cmd.Parameters.AddWithValue("@TopSpeed", topSpeed);
                cmd.Parameters.AddWithValue("@BatteryCapacity", battery);
                cmd.Parameters.AddWithValue("@StateOfCharge", charge);
                cmd.Parameters.AddWithValue("@TargetRange", totalRange);
                cmd.Parameters.AddWithValue("@Location", location);
                cmd.Parameters.AddWithValue("@OutOfService", outOfService);
                cmd.Parameters.AddWithValue("@Mileage", mileage);
                cmd.Parameters.AddWithValue("@LastMaintenance", lastMaintenance);

                cmd.ExecuteNonQuery();
            }
        }
    }
    public void UpdateScooter(int scooterId, string brand, string model, int topSpeed, int battery, int charge, int totalRange, string location, int outOfService, int mileage, DateTime lastMaintenance)
    {
        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();
            string sql = @"UPDATE Scooter SET 
                            Brand = @Brand,
                            Model = @Model,
                            TopSpeed = @TopSpeed,
                            BatteryCapacity = @BatteryCapacity,
                            StateOfCharge = @StateOfCharge,
                            TargetRange = @TargetRange,
                            Location = @Location,
                            OutOfService = @OutOfService,
                            Mileage = @Mileage,
                            LastMaintenance = @LastMaintenance
                        WHERE Id = @Id";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Brand", brand);
                cmd.Parameters.AddWithValue("@Model", model);
                cmd.Parameters.AddWithValue("@TopSpeed", topSpeed);
                cmd.Parameters.AddWithValue("@BatteryCapacity", battery);
                cmd.Parameters.AddWithValue("@StateOfCharge", charge);
                cmd.Parameters.AddWithValue("@TargetRange", totalRange);
                cmd.Parameters.AddWithValue("@Location", location);
                cmd.Parameters.AddWithValue("@OutOfService", outOfService);
                cmd.Parameters.AddWithValue("@Mileage", mileage);
                cmd.Parameters.AddWithValue("@LastMaintenance", lastMaintenance);
                cmd.Parameters.AddWithValue("@Id", scooterId);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public void DeleteScooter(int scooterId)
    {
        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();
            string sql = "DELETE FROM Scooter WHERE Id = @Id";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", scooterId);
                cmd.ExecuteNonQuery();
            }
        }
    }

   
    
    public string Login(string username, string password)
    {
        string passwordHash = HashPassword(password);

        using (var conn = new SQLiteConnection(connection))
        {
            conn.Open();
            string sql = "SELECT Role FROM User WHERE Username = @Username AND PasswordHash = @PasswordHash";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                object result = cmd.ExecuteScalar();

                return result?.ToString();
            }
        }
    }

    




























    private string HashPassword(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }

    private string Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _aesKey;
            aes.IV = _aesIV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(encrypted);
        }
    }


}
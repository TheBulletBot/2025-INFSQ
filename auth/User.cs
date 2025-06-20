using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


public abstract class User
{
    public string Username { get; }
    public string Role { get; }

    public User(string username, string role)
    {
        this.Username = username;
        this.Role = role;
    }
    public abstract void Menu();
}

public class ServiceEngineer : User
{
    public ServiceEngineer(string username, string role) : base(username, role)
    {

    }
    //Insert Menus for Service Engineers here.
    public override void Menu()
    {

    }

    public void UpdateScooter(string id, string brand, string model, int topSpeed, int battery, int charge, int totalRange, string location, int outOfService, int mileage, DateTime lastMaintenance)
    {
        string formattedDate = lastMaintenance.ToString("yyyy-MM-dd");

        string sql = $@"
            UPDATE Scooter
            SET Brand = '{brand}',
                Model = '{model}',
                TopSpeed = {topSpeed},
                BatteryCapacity = {battery},
                StateOfCharge = {charge},
                TargetRange = {totalRange},
                Location = '{location}',
                OutOfService = {outOfService},
                Mileage = {mileage},
                LastMaintenance = '{formattedDate}'
            WHERE Id = '{id}'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Scooter succesvol bijgewerkt.");
    }
    public void UpdateOwnPassword(string username, string newPassword) //(string OwnUserName)
    {
        // kan nu nog alle ww veranderen 
        string passwordHash = CryptoHelper.HashPassword(newPassword);
        string sql = $"UPDATE User SET PasswordHash = '{passwordHash}' WHERE Username = '{username}'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Je account is bijgewerkt.");
    }

    //Search Scooter Function!!!

    
    //Insert all functions that Service Engineers can perform here. 



}

public class Admin : ServiceEngineer
{
    private readonly string connection =
        @"Data Source=C:\Users\rensg\OneDrive\Documenten\GitHub\2025-INFSQ-2\ScooterBackend\db\db\INFSQScooterBackend.db";
    private readonly byte[] _aesKey = Encoding.UTF8.GetBytes("1234567890ABCDEF");   
    private readonly byte[] _aesIV = Encoding.UTF8.GetBytes("FEDCBA0987654321");   
    public Admin(string username, string role) : base(username, role)
    {

    }
    //Insert Menus for Admins here
        public override void Menu()
    {
        
    }
    //Insert all functions that Only Admins can perform here


    public void AddTraveler(string username, string password,
        string firstName, string lastName, DateTime birthday, string gender,
        string street, string houseNumber, string zipCode, string city,
        string email, string phone, string license)
    {
        if (!Regex.IsMatch(zipCode, @"^\d{4}[A-Z]{2}$"))
            throw new ArgumentException("Ongeldige postcode");
        if (!Regex.IsMatch(phone, @"^\d{8}$"))
            throw new ArgumentException("Ongeldig telefoonnummer");
        if (!Regex.IsMatch(license, @"^[A-Z]{1,2}\d{7}$"))
            throw new ArgumentException("Ongeldig rijbewijsnummer");
        if (password.Length < 4)
            throw new ArgumentException("Wachtwoord moet minimaal 4 tekens zijn.");

        string passwordHash = CryptoHelper.HashPassword(password);
        string registrationDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string encryptedPhone = CryptoHelper.Encrypt(phone);
        string encryptedStreet = CryptoHelper.Encrypt(street);
        string encryptedCity = CryptoHelper.Encrypt(city);

        string sql = $@"INSERT INTO Traveller 
            (Id, Username, PasswordHash, FirstName, LastName, Birthday, Gender,
             Street, HouseNumber, ZipCode, City, Mail, Phone, LicenseNumber, RegistrationDate)
            VALUES
            ('{Guid.NewGuid()}', '{username}', '{passwordHash}', '{firstName}', '{lastName}', '{birthday:yyyy-MM-dd}', '{gender}',
             '{encryptedStreet}', '{houseNumber}', '{zipCode}', '{encryptedCity}', '{email}', '{encryptedPhone}', '{license}', '{registrationDate}')";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Traveller succesvol toegevoegd.");
    }

    public void ShowAllUsersAndRoles()
    {
        string sql = "SELECT Id, Username, Role FROM User ORDER BY Role, Username";
        var users = DatabaseHelper.Query<DBUser>(sql);

        Console.WriteLine("\n--- Gebruikers en Rollen ---\n");
        if (users.Count == 0)
        {
            Console.WriteLine(" Geen gebruikers gevonden.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine($"UserName: {user.Username,-15} | Rol: {user.Role}");
        }
    }

    public void AddEngineer(string username, string password)
    {
        string passwordHash = CryptoHelper.HashPassword(password);
        string sql = $"INSERT INTO User (Id, Username, PasswordHash, Role) VALUES ('{Guid.NewGuid()}', '{username}', '{passwordHash}', 'Service Engineer')";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Service Engineer toegevoegd!");
    }

    public void UpdateEngineer(string currentUsername, string newUsername, string newPassword)
    {
        string passwordHash = CryptoHelper.HashPassword(newPassword);
        string sql = $"UPDATE User SET Username = '{newUsername}', PasswordHash = '{passwordHash}' WHERE Username = '{currentUsername}' AND Role = 'Service Engineer'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Service Engineer bijgewerkt.");
    }

    public void DeleteEngineer(string username)
    {
        string sql = $"DELETE FROM User WHERE Username = '{username}' AND Role = 'Service Engineer'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Service Engineer verwijderd: " + username);
    }

    public void ResetEngineerPassword(string username)
    {
        string tempPassword = "Temp" + new Random().Next(1000, 9999);
        string passwordHash = CryptoHelper.HashPassword(tempPassword);
        string sql = $"UPDATE User SET PasswordHash = '{passwordHash}' WHERE Username = '{username}' AND Role = 'Service Engineer'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine($"Tijdelijk wachtwoord ingesteld voor {username}: {tempPassword}");
    }

    

    public void DeleteOwnAccount(string username)
    {
        string sql = $"DELETE FROM User WHERE Username = '{username}'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Je account is verwijderd.");
    }

    public void SearchAndPrintTravellers(string searchKey)
    {
        string sql = $@"SELECT * FROM Traveller WHERE FirstName LIKE '%{searchKey}%' OR LastName LIKE '%{searchKey}%' OR Phone LIKE '%{searchKey}%' OR Mail LIKE '%{searchKey}%' OR LicenseNumber LIKE '%{searchKey}%'";
        var travellers = DatabaseHelper.Query<Traveler>(sql);

        Console.WriteLine("\n--- Zoekresultaten ---\n");
        if (travellers.Count == 0)
        {
            Console.WriteLine(" Geen reizigers gevonden ");
            return;
        }
        foreach (var t in travellers)
        {
            Console.WriteLine($"ID: {t.Id}\nNaam: {t.FirstName} {t.LastName}\nGeboortedatum: {t.Birthday}\nGeslacht: {t.Gender}\nAdres: {t.StreetName} {t.HouseNumber}, {t.ZipCode} {t.City}\nMail: {t.Email}\nTelefoon: {t.Phone}\nRijbewijs: {t.LicenseNumber}\n--------------------------\n");
        }
    }
    public void UpdateTraveller(string oldFirstName, string oldLastName, string oldPhoneNumber,
                            string newUsername, string newPassword, string newFirstName, string newLastName, DateTime newBirthDate,
                            string newGender, string newStreet, string newHouseNumber, string newZipCode, string newCity,
                            string newEmail, string newPhoneNumber, string newLicenseNumber)
    {
        if (!Regex.IsMatch(newZipCode, @"^\d{4}[A-Z]{2}$"))
            throw new ArgumentException("Ongeldige postcode");
        if (!Regex.IsMatch(newPhoneNumber, @"^\d{8}$"))
            throw new ArgumentException("Ongeldig telefoonnummer");
        if (!Regex.IsMatch(newLicenseNumber, @"^[A-Z]{1,2}\d{7}$"))
            throw new ArgumentException("Ongeldig rijbewijsnummer");
        if (newPassword.Length < 4)
            throw new ArgumentException("Wachtwoord moet minimaal 4 tekens zijn.");

        string passwordHash = CryptoHelper.HashPassword(newPassword);

        string encryptedPhone = CryptoHelper.Encrypt(newPhoneNumber);
        string encryptedStreet = CryptoHelper.Encrypt(newStreet);
        string encryptedCity = CryptoHelper.Encrypt(newCity);

        string oldEncryptedPhone = CryptoHelper.Encrypt(oldPhoneNumber);

        string sql = $@"
            UPDATE Traveller SET
                Username = '{newUsername}',
                PasswordHash = '{passwordHash}',
                FirstName = '{newFirstName}',
                LastName = '{newLastName}',
                Birthday = '{newBirthDate:yyyy-MM-dd}',
                Gender = '{newGender}',
                Street = '{encryptedStreet}',
                HouseNumber = '{newHouseNumber}',
                ZipCode = '{newZipCode}',
                City = '{encryptedCity}',
                Mail = '{newEmail}',
                Phone = '{encryptedPhone}',
                LicenseNumber = '{newLicenseNumber}'
            WHERE FirstName = '{oldFirstName}' AND LastName = '{oldLastName}' AND Phone = '{oldEncryptedPhone}'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Traveller succesvol bijgewerkt.");
    }


    public void DeleteTraveller(string firstName, string lastName, string phoneNumber)
    {
        string encryptedPhone = CryptoHelper.Encrypt(phoneNumber);
        string sql = $"DELETE FROM Traveller WHERE FirstName = '{firstName}' AND LastName = '{lastName}' AND Phone = '{encryptedPhone}'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine($"Traveller verwijderd: {firstName} {lastName}");
    }
    public void AddScooter(string brand, string model, int topSpeed, int battery, int charge, int totalRange, string location, int outOfService, int mileage, DateTime lastMaintenance)
    {
        string formattedDate = lastMaintenance.ToString("yyyy-MM-dd");

        string sql = $@"
            INSERT INTO Scooter 
            (Brand, Model, TopSpeed, BatteryCapacity, StateOfCharge, TargetRange, Location, OutOfService, Mileage, LastMaintenance)
            VALUES
            ('{brand}', '{model}', {topSpeed}, {battery}, {charge}, {totalRange}, '{location}', {outOfService}, {mileage}, '{formattedDate}')
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Scooter succesvol toegevoegd.");
    }
    public void DeleteScooter(string scooterId)
    {
        string sql = $"DELETE FROM Scooter WHERE Id = '{scooterId}'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine($"Scooter met ID {scooterId} is verwijderd.");
    }

   


    
}

public class SuperAdmin : Admin
{
    public SuperAdmin(string username, string role) : base(username, role)
    {

    }
    //Insert Menus for Admins here
    public override void Menu()
    {

    }
    public void AddSystemAdministrator(string username, string password)
    {
        string passwordHash = CryptoHelper.HashPassword(password);
        string id = Guid.NewGuid().ToString();

        string sql = $@"
            INSERT INTO User (Id, Username, PasswordHash, Role)
            VALUES ('{id}', '{username}', '{passwordHash}', 'System Administrator')
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("System Administrator toegevoegd!");
    }

    public void UpdateSystemAd(string currentUsername, string newUsername, string newPassword)
    {
        string passwordHash = CryptoHelper.HashPassword(newPassword);

        string sql = $@"
            UPDATE User
            SET Username = '{newUsername}', PasswordHash = '{passwordHash}'
            WHERE Username = '{currentUsername}' AND Role = 'System Administrator'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("System Administrator bijgewerkt (of niet gevonden).");
    }
    public void DeleteSystemAd(string username)
    {
        string sql = $@"
            DELETE FROM User
            WHERE Username = '{username}' AND Role = 'System Administrator'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine($"System Administrator verwijderd: {username}");
    }

    public void ResetSystemAdPassword(string username)
    {
        string tempPassword = "Temp" + new Random().Next(1000, 9999);
        string passwordHash = CryptoHelper.HashPassword(tempPassword);

        string sql = $@"
            UPDATE User
            SET PasswordHash = '{passwordHash}'
            WHERE Username = '{username}' AND Role = 'System Administrator'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine($"Tijdelijk wachtwoord voor {username}: {tempPassword}");
    }
}
    public static class CryptoHelper
    {
        
        private static readonly byte[] aesKey = Encoding.UTF8.GetBytes("1234567890ABCDEF");
        private static readonly byte[] aesIV = Encoding.UTF8.GetBytes("FEDCBA0987654321");

        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }
        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIV;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                return Convert.ToBase64String(encryptedBytes);
            }
        }
        public static string Decrypt(string encryptedText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIV;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }



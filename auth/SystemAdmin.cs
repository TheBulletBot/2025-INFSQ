
using System.Text;
using System.Text.RegularExpressions;

public class SystemAdmin : ServiceEngineer
{
    public SystemAdmin(string username, string role) : base(username, role) { }

    public override void Menu()
    {
        string[] options = {
        "Change password",
        "Edit scooter attributes",
        "Search scooter",
        "View all users",
        "Add new Service Engineer",
        "Update Service Engineer",
        "Delete Service Engineer",
        "Reset Service Engineer password",
        "Update own profile",
        "Delete own account",
        "Backup system",
        "Restore backup with code",
        "View logs",
        "Add new Traveler",
        "Update Traveler info",
        "Delete Traveler",
        "Add new Scooter",
        "Update Scooter info",
        "Delete Scooter",
        "Search Traveler",
        "Return"
    };

        int selection = 0;
        ConsoleKey key;

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== System Administrator Menu ({Username}) ===");

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selection)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            if (key == ConsoleKey.UpArrow)
                selection = (selection - 1 + options.Length) % options.Length;
            else if (key == ConsoleKey.DownArrow)
                selection = (selection + 1) % options.Length;
            else if (key == ConsoleKey.Enter)
            {
                /*switch (selection)
                {
                    case 0: UpdateOwnPassword(); break;
                    case 1: UpdateScooter(); break;
                    case 2: SearchScooter(); break;
                    case 3: ShowAllUsersAndRoles(); break;
                    case 4: AddEngineer(); break;
                    case 5: UpdateEngineer(); break;
                    case 6: DeleteEngineer(); break;
                    case 7: ResetEngineerPassword(); break;
                    case 8: UpdateOwnPassword(); break;
                    case 9: DeleteOwnAccount(); break;
                    case 10: BackupSystem(); break;
                    case 11: RestoreSystem(); break;
                    case 12: ViewLogs(); break;
                    case 13: AddTraveler(); break;
                    case 14: UpdateTraveller(); break;
                    case 15: DeleteTraveller(); break;
                    case 16: AddScooter(); break;
                    case 17: UpdateScooter(); break;
                    case 18: DeleteScooter(); break;
                    case 19: SearchAndPrintTravellers(); break;
                    case 20: return;
                }*/
            }
        }
    }

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

        string passwordHash = CryptographyHelper.CreateHashValue(password);
        string registrationDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string encryptedPhone = CryptographyHelper.Encrypt(phone);
        string encryptedStreet = CryptographyHelper.Encrypt(street);
        string encryptedCity = CryptographyHelper.Encrypt(city);

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
        string passwordHash = CryptographyHelper.CreateHashValue(password);
        string sql = $"INSERT INTO User (Id, Username, PasswordHash, Role) VALUES ('{Guid.NewGuid()}', '{username}', '{passwordHash}', 'Service Engineer')";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Service Engineer toegevoegd!");
    }

    public void UpdateEngineer(string currentUsername, string newUsername, string newPassword)
    {
        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);
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
        string passwordHash = CryptographyHelper.CreateHashValue(tempPassword);
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

        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);

        string encryptedPhone = CryptographyHelper.Encrypt(newPhoneNumber);
        string encryptedStreet = CryptographyHelper.Encrypt(newStreet);
        string encryptedCity = CryptographyHelper.Encrypt(newCity);

        string oldEncryptedPhone = CryptographyHelper.Encrypt(oldPhoneNumber);

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
        string encryptedPhone = CryptographyHelper.Encrypt(phoneNumber);
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
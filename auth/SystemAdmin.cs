
using System.Text;
using System.Text.RegularExpressions;

public class SystemAdmin : ServiceEngineer
{
    public SystemAdmin(string username, string role) : base(username, role) { }
    private Dictionary<string, string> backupcode = new();

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
                switch (selection)
                {
                    case 0: UpdateOwnPasswordMenu(); break;
                    case 1: UpdateScooterMenu(); break;
                    case 2: SearchScooterMenu(); break; 
                    case 3: ShowAllUsersAndRoles(); break;
                    case 4: AddEngineerMenu(); break;
                    case 5: UpdateEngineerMenu(); break;
                    case 6: DeleteEngineerMenu(); break;
                    case 7: ResetEngineerPasswordMenu(); break;
                    //case 8: UpdateOwnPasswordMenu(); break; ???
                    case 9: DeleteOwnAccountMenu(); break;
                    case 10: BackupMenu(); break;
                    case 11: BackupRestoreMenu(); break;
                    //case 12: ViewLogs(); break; bestaat niet
                    case 13: AddTravelerMenu(); break;
                    case 14: UpdateTravelerMenu(); break;
                    case 15: DeleteTravelerMenu(); break;
                    case 16: AddScooterMenu(); break;
                    case 17: UpdateScooterMenu(); break;
                    case 18: DeleteScooterMenu(); break;
                    case 19: SearchAndPrintTravelersMenu(); break;
                    case 20: return;
                }
            }
        }
    }

    public void AddTravelerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Voeg Traveler toe ===");

        Console.Write("Gebruikersnaam: ");
        string username = Console.ReadLine();

        Console.Write("Wachtwoord: ");
        string password = Console.ReadLine();

        Console.Write("Voornaam: ");
        string firstName = Console.ReadLine();

        Console.Write("Achternaam: ");
        string lastName = Console.ReadLine();

        Console.Write("Geboortedatum (yyyy-MM-dd): ");
        DateTime birthday;
        while (!DateTime.TryParse(Console.ReadLine(), out birthday))
        {
            Console.Write("Ongeldige invoer. Probeer opnieuw (yyyy-MM-dd): ");
        }

        Console.Write("Geslacht (M/V): ");
        string gender = Console.ReadLine();

        Console.Write("Straat: ");
        string street = Console.ReadLine();

        Console.Write("Huisnummer: ");
        string houseNumber = Console.ReadLine();

        Console.Write("Postcode (1234AB): ");
        string zipCode = Console.ReadLine();

        Console.Write("Woonplaats: ");
        string city = Console.ReadLine();

        Console.Write("E-mailadres: ");
        string email = Console.ReadLine();

        Console.Write("Telefoonnummer (8 cijfers): ");
        string phone = Console.ReadLine();

        Console.Write("Rijbewijsnummer (A1234567): ");
        string license = Console.ReadLine();

        try
        {
            AddTraveler(username, password, firstName, lastName, birthday, gender, street, houseNumber, zipCode, city, email, phone, license);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij toevoegen van traveler: {ex.Message}");
        }

        Console.WriteLine("Druk op een toets om terug te keren naar het menu.");
        Console.ReadKey();
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
        if (!Regex.IsMatch(password, Validation.PasswordRe))
            throw new ArgumentException("Wachtwoord moet minimaal 12 tekens zijn.");

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
        Logging.Log(Username, "AddTraveler", $"Traveler {username} added", false);
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

    public void AddEngineerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Voeg Service Engineer toe ===");

        Console.Write("Gebruikersnaam: ");
        string username = Console.ReadLine();

        Console.Write("Wachtwoord: ");
        string password = Console.ReadLine();

        Console.Write("Voornaam: ");
        string firstName = Console.ReadLine();

        Console.Write("Achternaam: ");
        string lastName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Alle velden zijn verplicht.");
            return;
        }

        AddEngineer(username, password, firstName, lastName);
    }


    public void AddEngineer(string username, string password, string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Alle velden zijn verplicht.");
            return;
        }

        if (!Regex.IsMatch(password, Validation.PasswordRe))
        {
            Console.WriteLine("Wachtwoord voldoet niet aan de eisen.");
            return;
        }

        string passwordHash = CryptographyHelper.CreateHashValue(password);
        string registrationDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string sql = $@"
            INSERT INTO User (Id, Username, PasswordHash, Role, FirstName, LastName, RegistrationDate)
            VALUES ('{Guid.NewGuid()}', '{username}', '{passwordHash}', 'Service Engineer', '{firstName}', '{lastName}', '{registrationDate}')
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "AddEngineer", $"Engineer {username} added", false);
        Console.WriteLine("Service Engineer succesvol toegevoegd!");
    }


    public void UpdateEngineerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Pas Service Engineer aan ===");
        Console.Write("Huidige gebruikersnaam: ");
        string currentUsername = Console.ReadLine();
        Console.Write("Nieuwe gebruikersnaam: ");
        string newUsername = Console.ReadLine();
        Console.Write("Nieuwe wachtwoord: ");
        string newPassword = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(currentUsername) || string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newPassword))
        {
            Console.WriteLine("Gebruikersnaam en wachtwoord mogen niet leeg zijn.");
            return;
        }

        UpdateEngineer(currentUsername, newUsername, newPassword);
    }

    public void UpdateEngineer(string currentUsername, string newUsername, string newPassword)
    {
        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);
        string sql = $"UPDATE User SET Username = '{newUsername}', PasswordHash = '{passwordHash}' WHERE Username = '{currentUsername}' AND Role = 'Service Engineer'";
        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(this.Username, "Update Engineer", $"Updated: {currentUsername}", false);
        Console.WriteLine("Service Engineer bijgewerkt.");
    }

    public void DeleteEngineerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder Service Engineer ===");
        Console.Write("Gebruikersnaam: ");
        string username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Gebruikersnaam mag niet leeg zijn.");
            return;
        }

        DeleteEngineer(username);
    }

    public void DeleteEngineer(string username)
    {
        string sql = $"DELETE FROM User WHERE Username = '{username}' AND Role = 'Service Engineer'";
        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "DeleteEngineer", $"Engineer {username} deleted", true);
        Console.WriteLine("Service Engineer verwijderd: " + username);
    }

    public void ResetEngineerPasswordMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Reset Service Engineer wachtwoord ===");
        Console.Write("Gebruikersnaam: ");
        string username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Gebruikersnaam mag niet leeg zijn.");
            return;
        }

        DeleteEngineer(username);
    }

    public void ResetEngineerPassword(string username)
    {
        string tempPassword = "Temp" + new Random().Next(1000, 9999);
        string passwordHash = CryptographyHelper.CreateHashValue(tempPassword);
        string sql = $"UPDATE User SET PasswordHash = '{passwordHash}' WHERE Username = '{username}' AND Role = 'Service Engineer'";
        Logging.Log(Username, "ResetEngineerPassword", $"Password reset for {username}", true);
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine($"Tijdelijk wachtwoord ingesteld voor {username}: {tempPassword}");
    }

    public void DeleteOwnAccountMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder eigen account ===");
        Console.Write("Bevestig gebruikersnaam: ");
        string username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Gebruikersnaam mag niet leeg zijn.");
            return;
        }
        Logging.Log(Username, "DeleteOwnAccount", $"Deleted own account: {username}", true);

        DeleteOwnAccount(username);
    }

    public void DeleteOwnAccount(string username)
    {
        string sql = $"DELETE FROM User WHERE Username = '{username}'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Je account is verwijderd.");
    }

    public void SearchAndPrintTravelersMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Zoek travelers op searchkey ===");
        Console.Write("Bevestig searchkey: ");
        string searchKey = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(searchKey))
        {
            Console.WriteLine("Searchkey mag niet leeg zijn.");
            return;
        }

        DeleteOwnAccount(searchKey);
    }

    public void SearchAndPrintTravelers(string searchKey)
    {
        string sql = $@"SELECT * FROM Traveler WHERE FirstName LIKE '%{searchKey}%' OR LastName LIKE '%{searchKey}%' OR Phone LIKE '%{searchKey}%' OR Mail LIKE '%{searchKey}%' OR LicenseNumber LIKE '%{searchKey}%'";
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

    public void UpdateTravelerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Pas traveler aan ===");

        // Oude gegevens voor identificatie
        Console.WriteLine("Voer de huidige gegevens in van de traveler die je wilt bijwerken:");
        Console.Write("Oude voornaam: ");
        string oldFirstName = Console.ReadLine();

        Console.Write("Oude achternaam: ");
        string oldLastName = Console.ReadLine();

        Console.Write("Oud telefoonnummer (8 cijfers): ");
        string oldPhoneNumber = Console.ReadLine();

        // Nieuwe gegevens
        Console.WriteLine("\nVoer de nieuwe gegevens in:");
        Console.Write("Nieuwe gebruikersnaam: ");
        string newUsername = Console.ReadLine();

        Console.Write("Nieuw wachtwoord: ");
        string newPassword = Console.ReadLine();

        Console.Write("Nieuwe voornaam: ");
        string newFirstName = Console.ReadLine();

        Console.Write("Nieuwe achternaam: ");
        string newLastName = Console.ReadLine();

        Console.Write("Nieuwe geboortedatum (yyyy-MM-dd): ");
        DateTime newBirthDate;
        while (!DateTime.TryParse(Console.ReadLine(), out newBirthDate))
        {
            Console.Write("Ongeldige invoer. Probeer opnieuw (yyyy-MM-dd): ");
        }

        Console.Write("Nieuw geslacht (M/V): ");
        string newGender = Console.ReadLine();

        Console.Write("Nieuwe straat: ");
        string newStreet = Console.ReadLine();

        Console.Write("Nieuw huisnummer: ");
        string newHouseNumber = Console.ReadLine();

        Console.Write("Nieuwe postcode (1234AB): ");
        string newZipCode = Console.ReadLine();

        Console.Write("Nieuwe woonplaats: ");
        string newCity = Console.ReadLine();

        Console.Write("Nieuw e-mailadres: ");
        string newEmail = Console.ReadLine();

        Console.Write("Nieuw telefoonnummer (8 cijfers): ");
        string newPhoneNumber = Console.ReadLine();

        Console.Write("Nieuw rijbewijsnummer (A1234567): ");
        string newLicenseNumber = Console.ReadLine();

        try
        {
            UpdateTraveler(oldFirstName, oldLastName, oldPhoneNumber,
                newUsername, newPassword, newFirstName, newLastName, newBirthDate,
                newGender, newStreet, newHouseNumber, newZipCode, newCity,
                newEmail, newPhoneNumber, newLicenseNumber);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij bijwerken van traveler: {ex.Message}");
        }

        Console.WriteLine("Druk op een toets om terug te keren naar het menu.");
        Console.ReadKey();
    }

    public void UpdateTraveler(string oldFirstName, string oldLastName, string oldPhoneNumber,
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
            UPDATE Traveler SET
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
        Logging.Log(this.Username, "Update Traveller", $"Updated {oldFirstName} {oldLastName}", false);
        Console.WriteLine("Traveler succesvol bijgewerkt.");
    }

    public void DeleteTravelerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder Traveler ===");

        Console.Write("Voornaam: ");
        string firstName = Console.ReadLine();

        Console.Write("Achternaam: ");
        string lastName = Console.ReadLine();

        Console.Write("Telefoonnummer (8 cijfers): ");
        string phoneNumber = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber))
        {
            Console.WriteLine("Voornaam, achternaam en telefoonnummer mogen niet leeg zijn.");
            return;
        }

        DeleteTraveler(firstName, lastName, phoneNumber);
    }

    public void DeleteTraveler(string firstName, string lastName, string phoneNumber)
    {
        string encryptedPhone = CryptographyHelper.Encrypt(phoneNumber);
        string sql = $"DELETE FROM Traveler WHERE FirstName = '{firstName}' AND LastName = '{lastName}' AND Phone = '{encryptedPhone}'";
        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "DeleteTraveler", $"Traveler {firstName} {lastName} deleted", true);
        Console.WriteLine($"Traveler verwijderd: {firstName} {lastName}");
    }

    public void AddScooterMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Voeg Scooter toe ===");

        Console.Write("Merk: ");
        string brand = Console.ReadLine();

        Console.Write("Model: ");
        string model = Console.ReadLine();

        Console.Write("Maximale snelheid (km/u): ");
        int topSpeed = int.Parse(Console.ReadLine());

        Console.Write("Batterijcapaciteit (Wh): ");
        int battery = int.Parse(Console.ReadLine());

        Console.Write("Huidige lading (%): ");
        int charge = int.Parse(Console.ReadLine());

        Console.Write("Totale actieradius (km): ");
        int totalRange = int.Parse(Console.ReadLine());

        Console.Write("Locatie: ");
        string location = Console.ReadLine();

        Console.Write("Niet in gebruik (0/1): ");
        int outOfService = int.Parse(Console.ReadLine());

        Console.Write("Kilometerstand: ");
        int mileage = int.Parse(Console.ReadLine());

        Console.Write("Datum laatste onderhoud (YYYY-MM-DD): ");
        DateTime lastMaintenance;
        while (!DateTime.TryParse(Console.ReadLine(), out lastMaintenance))
        {
            Console.Write("Ongeldige datum. Probeer opnieuw (YYYY-MM-DD): ");
        }

        AddScooter(brand, model, topSpeed, battery, charge, totalRange, location, outOfService, mileage, lastMaintenance);

        Console.WriteLine("\nDruk op een toets om terug te keren naar het menu");
        Console.ReadKey();
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
        Logging.Log(Username, "AddScooter", $"Scooter {brand} {model} added", false);
        Console.WriteLine("Scooter succesvol toegevoegd.");
    }

    public void DeleteScooterMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder Scooter ===");
        Console.Write("Voer het ID van de scooter in: ");
        string scooterId = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(scooterId))
        {
            Console.WriteLine("Scooter ID mag niet leeg zijn.");
            return;
        }

        DeleteScooter(scooterId);
    }

    public void DeleteScooter(string scooterId)
    {
        string sql = $"DELETE FROM Scooter WHERE Id = '{scooterId}'";
        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "DeleteScooter", $"Scooter with ID {scooterId} deleted", true);
        Console.WriteLine($"Scooter met ID {scooterId} is verwijderd.");
    }
    public void CreateSystemBackup()
    {
        Console.Clear();
        string sourcePath = DatabaseHelper.DatabasePath;
        string backupDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../db/backups");
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string backupFileName = $"backup_{timestamp}.db";
        string destinationPath = Path.Combine(backupDir, backupFileName);

        if (!Directory.Exists(backupDir))
        {
            Directory.CreateDirectory(backupDir);
        }

        try
        {
            File.Copy(sourcePath, destinationPath);
            string backupcode1 = Guid.NewGuid().ToString().Substring(0, 8);
            backupcode[backupcode1] = destinationPath;
            Logging.Log(Username, "CreateBackup", $"Backup created: {backupFileName}", false);
            Console.WriteLine($" Backup succesvol opgeslagen als: {backupFileName}");
            Console.WriteLine($" Herstelcode (eenmalig): {backupcode1}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Backup mislukt: {ex.Message}");
        }

    }

    public void RestoreBackup()
    {
        Console.Clear();

        Console.WriteLine("=== Restore System from Backup ===");
        Console.Write("Enter the restore code: ");
        string code = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(code) || !backupcode.ContainsKey(code))
        {
            Console.WriteLine("Ongeldige code of code al gebruikt.");
        }
        else
        {
            string backupPath = backupcode[code];
            string originalPath = DatabaseHelper.DatabasePath;

            try
            {
                File.Copy(backupPath, originalPath, overwrite: true);
                Logging.Log(Username, "RestoreBackup", $"Restored from backup {backupPath}", true);
                Console.WriteLine(" Backup herstel gelukt.");
                backupcode.Remove(code);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Herstel mislukt : {ex.Message}");
            }
        }
    }

    public void BackupMenu()
    {
        Console.Clear();
        while (true)
        {

            Console.WriteLine("=== Backup maken ===");
            Console.Write("Wil je een backup maken? (j/n): ");
            string antwoord = Console.ReadLine()?.Trim().ToLower();

            if (antwoord == "j")
            {
                CreateSystemBackup();
                break;
            }
            else if (antwoord == "n")
            {
                break;
            }
        }
    }
    public void BackupRestoreMenu()
    {
        Console.Clear();
        while (true)
        {

            Console.WriteLine("=== Backup maken ===");
            Console.Write("Wil je een backup herstellen (j/n): ");
            string antwoord = Console.ReadLine()?.Trim().ToLower();

            if (antwoord == "j")
            {
                RestoreBackup();
                break;
            }
            else if (antwoord == "n")
            {
                break;
            }
        }
    }


}
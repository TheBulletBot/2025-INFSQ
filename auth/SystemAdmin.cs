
using System.ComponentModel;
using System.Data.SQLite;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Nodes;

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
                    case 12: ViewLogs(); break; 
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

        string firstName = Validation.ValidatedInput(
            Validation.NameRe,
            "Voornaam:",
            "Ongeldige voornaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        string lastName = Validation.ValidatedInput(
            Validation.NameRe,
            "Achternaam:",
            "Ongeldige achternaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

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

        string zipCode = Validation.ValidatedInput(
            Validation.ZipCodeRe,
            "Postcode (1234AB):",
            "Ongeldige postcode. Gebruik het formaat 1234AB."
        );

        string city = Validation.ValidatedInput(
            Validation.NameRe,
            "Woonplaats:",
            "Ongeldige woonplaats. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        string email = Validation.ValidatedInput(
            Validation.EmailRe,
            "E-mailadres:",
            "Ongeldig e-mailadres. Gebruik een geldig formaat zoals voorbeeld@domein.nl."
        );

        string phone = Validation.ValidatedInput(
            Validation.PhoneRe,
            "Telefoonnummer (8 cijfers):",
            "Ongeldig telefoonnummer. Gebruik precies 8 cijfers."
        );

        string license = Validation.ValidatedInput(
            Validation.LicenseRe,
            "Rijbewijsnummer (A1234567):",
            "Ongeldig rijbewijsnummer. Gebruik 1–2 hoofdletters gevolgd door 7 cijfers."
        );

        try
        {
            AddTraveler(firstName, lastName, birthday, gender, street, houseNumber, zipCode, city, email, phone, license);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij toevoegen van traveler: {ex.Message}");
        }

        Console.WriteLine("\nDruk op een toets om terug te keren naar het menu.");
        Console.ReadKey();
    }

    public void AddTraveler(
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

        string registrationDate = DateTime.UtcNow.ToString("dd-MM-yyyy");

        string encryptedPhone = CryptographyHelper.Encrypt(phone);
        string encryptedStreet = CryptographyHelper.Encrypt(street);
        string encryptedCity = CryptographyHelper.Encrypt(city);
        var id = Guid.NewGuid();

        string sql = @"INSERT INTO Traveller 
            (Id, FirstName, LastName, Birthday, Gender,
             Street, HouseNumber, ZipCode, City, Mail, Phone, LicenseNumber, RegistrationDate)
            VALUES
            (@id, @firstname, @lastname, '@Borth', @gender,
             @Estreet, @EHouseNr, @EZipcode, @ECity, @Email, @EPhone, @ELicense, @registrationDate)";

        var queryCommand = new SQLiteCommand(sql);
        queryCommand.Parameters.AddWithValue("@id", id);
        queryCommand.Parameters.AddWithValue("@firstname", CryptographyHelper.Encrypt(firstName));
        queryCommand.Parameters.AddWithValue("@lastname", CryptographyHelper.Encrypt(lastName));
        queryCommand.Parameters.AddWithValue("@Borth", birthday.ToString("dd-MM-yyyy"));
        queryCommand.Parameters.AddWithValue("@gender", gender);
        queryCommand.Parameters.AddWithValue("@Estreet", encryptedStreet);
        queryCommand.Parameters.AddWithValue("@EHouseNr", CryptographyHelper.Encrypt(houseNumber));
        queryCommand.Parameters.AddWithValue("@EZipcode", CryptographyHelper.Encrypt(zipCode));
        queryCommand.Parameters.AddWithValue("@ECity", encryptedCity);
        queryCommand.Parameters.AddWithValue("@Email", CryptographyHelper.Encrypt(email));
        queryCommand.Parameters.AddWithValue("@EPhone", encryptedPhone);
        queryCommand.Parameters.AddWithValue("@registrationDate", registrationDate);

        queryCommand.Parameters.AddWithValue("@ELicense", CryptographyHelper.Encrypt(license));
        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "AddTraveler", $"Traveler {id} added", false);
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
            Console.WriteLine($"UserName: {CryptographyHelper.Decrypt(user.Username),-15} | Rol: {user.Role}");
        }
        Logging.Log(this.Username, "Show All Users", $"Displayed each user and its role", false);
        Console.ReadKey();
    }

    public void AddEngineerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Voeg Service Engineer toe ===");

        string username = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );

        string password = Validation.ValidatedInput(
            Validation.PasswordRe,
            "Wachtwoord:",
            "Ongeldig wachtwoord. Het moet 12–30 tekens zijn, met minstens één hoofdletter, één cijfer en één speciaal teken."
        );

        string firstName = Validation.ValidatedInput(
            Validation.NameRe,
            "Voornaam:",
            "Ongeldige voornaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        string lastName = Validation.ValidatedInput(
            Validation.NameRe,
            "Achternaam:",
            "Ongeldige achternaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        AddEngineer(username, password, firstName, lastName);
        Logging.Log(this.Username, "Add Engineer", $"Nieuwe engineer toegevoegd met gebruikersnaam: {username}", false);
    }

    public void AddEngineer(string username, string password,string firstName, string lastName)
    {
        //check if unique
        var Query = new SQLiteCommand("Select * FROM User u WHERE u.Username = @username");
        Query.Parameters.AddWithValue("@username", username);
        var result = DatabaseHelper.Query<User>(Query);
        if (result.Count > 0)
        {
            Console.WriteLine("Username already in use, Try again");
            return;
        }
        string passwordHash = CryptographyHelper.CreateHashValue(password);
        string encryptedUsername = CryptographyHelper.Encrypt(username);
        var registrationDate = DateTime.Now.ToString("dd-MM-yyyy");
        string encryptedFirstName = CryptographyHelper.Encrypt(firstName);
        string encryptedLastName = CryptographyHelper.Encrypt(lastName);
        string sql = @"INSERT INTO User (Id, Username, PasswordHash, Role,FirstName, LastName, RegistrationDate) 
        VALUES (@id, @username, @password, 'Service Engineer',@firstname, @lastname, @registrationdate)";

        var queryCommand = new SQLiteCommand(sql);
        queryCommand.Parameters.AddWithValue("@id", Guid.NewGuid());
        queryCommand.Parameters.AddWithValue("@username", encryptedUsername);
        queryCommand.Parameters.AddWithValue("@password", passwordHash);
        queryCommand.Parameters.AddWithValue("@firstname", encryptedFirstName);
        queryCommand.Parameters.AddWithValue("@lastname", encryptedLastName);
        queryCommand.Parameters.AddWithValue("@registrationdate", registrationDate);

        DatabaseHelper.ExecuteStatement(queryCommand);
        Console.WriteLine("Service Engineer toegevoegd!");
    }


    public void UpdateEngineerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Pas Service Engineer aan ===");

        string currentUsername = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Huidige gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );

        string newUsername = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Nieuwe gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );

        string newPassword = Validation.ValidatedInput(
            Validation.PasswordRe,
            "Nieuwe wachtwoord:",
            "Ongeldig wachtwoord. Het moet 12–30 tekens zijn, met minstens één hoofdletter, één cijfer en één speciaal teken."
        );

        UpdateEngineer(currentUsername, newUsername, newPassword);
        Logging.Log(this.Username, "Update Engineer", $"Updated Engineer with (former)Username: {currentUsername}", false);
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

        string username = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );

        DeleteEngineer(username);
    }

    public void DeleteEngineer(string username)
    {
        string sql = $"DELETE FROM User WHERE Username = @username AND Role = 'Service Engineer'";
        var encryptedUsername = CryptographyHelper.Encrypt(username);
        var queryCommand = new SQLiteCommand(sql);
        queryCommand.Parameters.AddWithValue("@username", encryptedUsername);
        DatabaseHelper.ExecuteStatement(queryCommand);
        Console.WriteLine("Service Engineer verwijderd: " + username);
    }

    public void ResetEngineerPasswordMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Reset Service Engineer wachtwoord ===");

        string username = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );

        ResetEngineerPassword(username);
    }

    public void ResetEngineerPassword(string username)
    {
        string tempPassword = "Temp" + new Random().Next(1000, 9999);
        string passwordHash = CryptographyHelper.CreateHashValue(tempPassword);
        string sql = $"UPDATE User SET PasswordHash = @password WHERE Username = @username AND Role = 'Service Engineer'";
        var encryptedUsername = CryptographyHelper.Encrypt(username);
        var queryCommand = new SQLiteCommand(sql);
        queryCommand.Parameters.AddWithValue("@username", encryptedUsername);
        queryCommand.Parameters.AddWithValue("@password", passwordHash);
        Logging.Log(Username, "ResetEngineerPassword", $"Password reset for {username}", true);
        DatabaseHelper.ExecuteStatement(queryCommand);
        Console.WriteLine($"Tijdelijk wachtwoord ingesteld voor {username}: {tempPassword}");
    }

    public void DeleteOwnAccountMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder eigen account ===");

        string username = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Bevestig gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );
        Logging.Log(Username, "DeleteOwnAccount", $"Deleted own account: {username}", true);

        DeleteOwnAccount(username);
    }

    public void DeleteOwnAccount(string username)
    {
        string sql = $"DELETE FROM User WHERE Username = '{username}'";
        var queryCommand = new SQLiteCommand(sql);
        var encryptedUsername = CryptographyHelper.Encrypt(username);
        queryCommand.Parameters.AddWithValue("@username", encryptedUsername);
        DatabaseHelper.ExecuteStatement(queryCommand);
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
        string sql = $@"SELECT * FROM Traveler WHERE FirstName LIKE @searchKey OR LastName LIKE '%{searchKey}%' OR Phone LIKE '%{searchKey}%' OR Mail LIKE '%{searchKey}%' OR LicenseNumber LIKE '%{searchKey}%'";
        var queryCommand = new SQLiteCommand(sql);
        queryCommand.Parameters.AddWithValue("@searchKey", CryptographyHelper.Encrypt(searchKey));
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

        string oldFirstName = Validation.ValidatedInput(
            Validation.NameRe,
            "Oude voornaam:",
            "Ongeldige voornaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        string oldLastName = Validation.ValidatedInput(
            Validation.NameRe,
            "Oude achternaam:",
            "Ongeldige achternaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        string oldPhoneNumber = Validation.ValidatedInput(
            Validation.PhoneRe,
            "Oud telefoonnummer (8 cijfers):",
            "Ongeldig telefoonnummer. Gebruik exact 8 cijfers."
        );

        // Nieuwe gegevens
        Console.WriteLine("\nVoer de nieuwe gegevens in:");

        string newUsername = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Nieuwe gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );

        string newPassword = Validation.ValidatedInput(
            Validation.PasswordRe,
            "Nieuw wachtwoord:",
            "Ongeldig wachtwoord. Het moet 12–30 tekens zijn, met minstens één hoofdletter, één cijfer en één speciaal teken."
        );

        string newFirstName = Validation.ValidatedInput(
            Validation.NameRe,
            "Nieuwe voornaam:",
            "Ongeldige voornaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        string newLastName = Validation.ValidatedInput(
            Validation.NameRe,
            "Nieuwe achternaam:",
            "Ongeldige achternaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        // Datum blijft met eigen prompt omdat ValidatedInput dat niet doet
        Console.Write("Nieuwe geboortedatum (yyyy-MM-dd): ");
        DateTime newBirthDate;
        while (!DateTime.TryParse(Console.ReadLine(), out newBirthDate))
        {
            Console.Write("Ongeldige invoer. Probeer opnieuw (yyyy-MM-dd): ");
        }

        // Geslacht validatie met eigen prompt
        string newGender;
        do
        {
            Console.Write("Nieuw geslacht (M/V): ");
            newGender = Console.ReadLine()!.Trim().ToUpper();
            if (newGender != "M" && newGender != "V")
            {
                Console.WriteLine("Ongeldige invoer. Voer 'M' of 'V' in.");
            }
        } while (newGender != "M" && newGender != "V");

        // Overige velden zonder regex validatie
        Console.Write("Nieuwe straat: ");
        string newStreet = Console.ReadLine()!;

        Console.Write("Nieuw huisnummer: ");
        string newHouseNumber = Console.ReadLine()!;

        string newZipCode = Validation.ValidatedInput(
            Validation.ZipCodeRe,
            "Nieuwe postcode (1234AB):",
            "Ongeldige postcode. Formaat moet 1234AB zijn (4 cijfers gevolgd door 2 hoofdletters)."
        );

        Console.Write("Nieuwe woonplaats: ");
        string newCity = Console.ReadLine()!;

        string newEmail = Validation.ValidatedInput(
            Validation.EmailRe,
            "Nieuw e-mailadres:",
            "Ongeldig e-mailadres. Gebruik een geldig formaat, bijvoorbeeld: gebruiker@voorbeeld.com"
        );

        string newPhoneNumber = Validation.ValidatedInput(
            Validation.PhoneRe,
            "Nieuw telefoonnummer (8 cijfers):",
            "Ongeldig telefoonnummer. Gebruik exact 8 cijfers."
        );

        string newLicenseNumber = Validation.ValidatedInput(
            Validation.LicenseRe,
            "Nieuw rijbewijsnummer (A1234567):",
            "Ongeldig rijbewijsnummer. Moet 1 of 2 hoofdletters gevolgd door 7 cijfers zijn."
        );

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
        string encryptedNewUsername = CryptographyHelper.Encrypt(newUsername);
        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);

        string encryptedPhone = CryptographyHelper.Encrypt(newPhoneNumber);
        string encryptedStreet = CryptographyHelper.Encrypt(newStreet);
        string encryptedCity = CryptographyHelper.Encrypt(newCity);
        string encryptedNewFirstName = CryptographyHelper.Encrypt(newFirstName);
        var encryptedNewlastName = CryptographyHelper.Encrypt(newLastName);
        var encryptedHouseNumber = CryptographyHelper.Encrypt(newHouseNumber);
        var encryptedZip = CryptographyHelper.Encrypt(newZipCode);
        var encryptedMail = CryptographyHelper.Encrypt(newEmail);
        var encryptedLicense = CryptographyHelper.Encrypt(newLicenseNumber);

        string oldEncryptedPhone = CryptographyHelper.Encrypt(oldPhoneNumber);
        
        string sql = $@"
            UPDATE Traveler SET
                FirstName = @firstname,
                LastName = @lastname,
                Birthday = @Borth,
                Gender = @gender,
                Street = @Estreet,
                HouseNumber = @EHouseNr,
                ZipCode = @EZipcode,
                City = @ECity,
                Mail = @Email,
                Phone = @Ephone,
                LicenseNumber = @ELicense
            WHERE FirstName = @deadname AND LastName = @deadlastname AND Phone = @oldphone
        ";
        var queryCommand = new SQLiteCommand(sql);
        queryCommand.Parameters.AddWithValue("@firstname", encryptedNewFirstName);
        queryCommand.Parameters.AddWithValue("@lastname", encryptedNewlastName);
        queryCommand.Parameters.AddWithValue("@Borth", newBirthDate.ToString("dd-MM-yyyy"));
        queryCommand.Parameters.AddWithValue("@gender", newGender);
        queryCommand.Parameters.AddWithValue("@Estreet", encryptedStreet);
        queryCommand.Parameters.AddWithValue("@EHouseNr", encryptedHouseNumber);
        queryCommand.Parameters.AddWithValue("@EZipcode", encryptedZip);
        queryCommand.Parameters.AddWithValue("@ECity", encryptedCity);
        queryCommand.Parameters.AddWithValue("@Email", encryptedMail);
        queryCommand.Parameters.AddWithValue("@EPhone", encryptedPhone);

        queryCommand.Parameters.AddWithValue("@ELicense", encryptedLicense);
        queryCommand.Parameters.AddWithValue("@deadname", CryptographyHelper.Encrypt(oldFirstName));
        queryCommand.Parameters.AddWithValue("@EPhone", CryptographyHelper.Encrypt(oldLastName));
        queryCommand.Parameters.AddWithValue("@EPhone", oldEncryptedPhone);


        DatabaseHelper.ExecuteStatement(queryCommand);
        Logging.Log(this.Username, "Update Traveller", $"Updated {oldFirstName} {oldLastName}", false);
        Console.WriteLine("Traveler succesvol bijgewerkt.");
    }

    public void DeleteTravelerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder Traveler ===");

        string firstName = Validation.ValidatedInput(
            Validation.NameRe,
            "Voornaam:",
            "Ongeldige voornaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        string lastName = Validation.ValidatedInput(
            Validation.NameRe,
            "Achternaam:",
            "Ongeldige achternaam. Gebruik alleen letters, spaties, streepjes of apostrofs (2–30 tekens)."
        );

        string phoneNumber = Validation.ValidatedInput(
            Validation.PhoneRe,
            "Telefoonnummer (8 cijfers):",
            "Telefoonnummer moet exact 8 cijfers bevatten."
        );

        DeleteTraveler(firstName, lastName, phoneNumber);
    }

    public void DeleteTraveler(string firstName, string lastName, string phoneNumber)
    {
        string encryptedPhone = CryptographyHelper.Encrypt(phoneNumber);
        string sql = $"DELETE FROM Traveler WHERE FirstName = @firstname AND LastName = @lastname AND Phone = @Ephone";
        var queryCommand = new SQLiteCommand(sql);
        queryCommand.Parameters.AddWithValue("@firstname", CryptographyHelper.Encrypt(firstName));
        queryCommand.Parameters.AddWithValue("@lastname", CryptographyHelper.Encrypt(lastName));
        queryCommand.Parameters.AddWithValue("@EPhone", encryptedPhone);
        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "DeleteTraveler", $"Traveler {firstName} {lastName} deleted", true);
        Console.WriteLine($"Traveler verwijderd: {firstName} {lastName}");
    }

    public void AddScooterMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Scooter Toevoegen ===");

        string brand = Validation.ValidatedInput(
            Validation.BrandRe,
            "Merk:",
            "Ongeldig merk. Gebruik 2–20 letters, cijfers, spaties of streepjes."
        );

        string model = Validation.ValidatedInput(
            Validation.ModelRe,
            "Model:",
            "Ongeldig model. Gebruik 1–20 letters, cijfers, spaties of streepjes."
        );

        Console.Write("Maximale snelheid (km/u): ");
        int topSpeed;
        while (!int.TryParse(Console.ReadLine(), out topSpeed))
        {
            Console.Write("Ongeldige invoer. Voer een geldig getal in voor de topsnelheid: ");
        }

        Console.Write("Batterijcapaciteit (Wh): ");
        int battery;
        while (!int.TryParse(Console.ReadLine(), out battery))
        {
            Console.Write("Ongeldige invoer. Voer een geldig getal in voor de batterijcapaciteit: ");
        }

        Console.Write("Ladingstatus (%): ");
        int charge;
        while (!int.TryParse(Console.ReadLine(), out charge))
        {
            Console.Write("Ongeldige invoer. Voer een geldig percentage in voor de ladingstatus: ");
        }

        Console.Write("Totaal bereik (km): ");
        int totalRange;
        while (!int.TryParse(Console.ReadLine(), out totalRange))
        {
            Console.Write("Ongeldige invoer. Voer een geldig getal in voor het bereik: ");
        }

        string location = Validation.ValidatedInput(
            Validation.LocationRe,
            "Locatie:",
            "Ongeldige locatie. Gebruik 2–30 tekens, letters/cijfers/spaties/komma’s/punten/streepjes."
        );

        Console.Write("Buiten dienst? (1 = ja, 0 = nee): ");
        int outOfService;
        while (!int.TryParse(Console.ReadLine(), out outOfService) || (outOfService != 0 && outOfService != 1))
        {
            Console.Write("Ongeldige invoer. Voer 1 in voor ja of 0 voor nee: ");
        }

        Console.Write("Kilometerstand (km): ");
        int mileage;
        while (!int.TryParse(Console.ReadLine(), out mileage))
        {
            Console.Write("Ongeldige invoer. Voer een geldig getal in voor de kilometerstand: ");
        }

        Console.Write("Datum laatste onderhoud (YYYY-MM-DD): ");
        DateTime lastMaintenance;
        while (!DateTime.TryParse(Console.ReadLine(), out lastMaintenance))
        {
            Console.Write("Ongeldige datum. Probeer opnieuw (YYYY-MM-DD): ");
        }

        AddScooter(brand, model, topSpeed, battery, charge, totalRange, location, outOfService, mileage, lastMaintenance);

        Console.WriteLine("\nDruk op een toets om terug te keren naar het menu.");
        Console.ReadKey();
    }

    public void AddScooter(string brand, string model, int topSpeed, int battery, int charge, int totalRange, string location, int outOfService, int mileage, DateTime lastMaintenance)
    {
        string formattedDate = lastMaintenance.ToString("dd-MM-yyyy");

        string sql = $@"
            INSERT INTO Scooter 
            (SerialNumber,Brand, Model, TopSpeed, BatteryCapacity, StateOfCharge, TargetRange, Location, OutOfService, Mileage, LastMaintenance)
            VALUES
            (@serial, @brand, @model, @topSpeed, @battery, @charge, @totalrange, @location, @oos, @miles, @date)
        ";
        var queryCommand = new SQLiteCommand(sql);
        queryCommand.Parameters.AddWithValue("@brand", Guid.NewGuid());
        queryCommand.Parameters.AddWithValue("@brand", CryptographyHelper.Encrypt(brand));
        queryCommand.Parameters.AddWithValue("@model", CryptographyHelper.Encrypt(model));
        queryCommand.Parameters.AddWithValue("@topSpeed", CryptographyHelper.Encrypt(topSpeed.ToString()));
        queryCommand.Parameters.AddWithValue("@battery", CryptographyHelper.Encrypt(battery.ToString()));
        queryCommand.Parameters.AddWithValue("@charge", CryptographyHelper.Encrypt(charge.ToString()));
        queryCommand.Parameters.AddWithValue("@totalrange", CryptographyHelper.Encrypt(totalRange.ToString()));
        queryCommand.Parameters.AddWithValue("@location", CryptographyHelper.Encrypt(location));
        queryCommand.Parameters.AddWithValue("@oos", CryptographyHelper.Encrypt(outOfService.ToString()));
        queryCommand.Parameters.AddWithValue("@miles", CryptographyHelper.Encrypt(mileage.ToString()));
        queryCommand.Parameters.AddWithValue("@date", CryptographyHelper.Encrypt(formattedDate));

        DatabaseHelper.ExecuteStatement(queryCommand);
        Logging.Log(Username, "AddScooter", $"Scooter {brand} {model} added", false);
        Console.WriteLine("Scooter succesvol toegevoegd.");
    }

    public void DeleteScooterMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder Scooter ===");

        string scooterId = Validation.ValidatedInput(
            Validation.NoRegex,
            "Voer het Serienummer van de scooter in:",
            "Ongeldig Serienummer. Gebruik alleen cijfers."
        );

        DeleteScooter(scooterId);

        Console.WriteLine("\nDruk op een toets om terug te keren naar het menu.");
        Console.ReadKey();
    }

    public void DeleteScooter(string scooterId)
    {
        string sql = $"DELETE FROM Scooter WHERE SerialNumber = @serial";

        var queryCommand = new SQLiteCommand(sql);
        queryCommand.Parameters.AddWithValue("@serial", CryptographyHelper.Encrypt(scooterId));

        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "DeleteScooter", $"Scooter with Serial {scooterId} deleted", true);
        Console.WriteLine($"Scooter met Serienummer {scooterId} is verwijderd.");
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
    public void ViewLogs()
    {
        Console.Clear();
        Console.WriteLine("=== Logboek bekijken ===\n");
        string logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Logs");
        if (!Directory.Exists(logsDir))
        {
            Console.WriteLine("❌ Geen logmap gevonden.");
            Console.ReadKey();
            return;
        }
        string[] logFiles = Directory.GetFiles(logsDir, "*.log");
        if (logFiles.Length == 0)
        {
            Console.WriteLine(" Geen logbestanden gevonden.");
            Console.ReadKey();
            return;
        }
        foreach (var file in logFiles.OrderBy(f => f))
        {
            Console.WriteLine($"\n--- Logbestand: {Path.GetFileName(file)} ---\n");
            try
            {
                string encryptedContent = File.ReadAllText(file);
                string decryptedContent = CryptographyHelper.Decrypt(encryptedContent);
                var logs = JsonSerializer.Deserialize<List<Logging.LogItem>>(decryptedContent);

                foreach (var log in logs)
                {
                    Console.WriteLine($"ID: {log.Id}");
                    Console.WriteLine($"Datum: {log.Date} {log.Time}");
                    Console.WriteLine($"Gebruiker: {log.Username}");
                    Console.WriteLine($"Actie: {log.Action}");
                    Console.WriteLine($"Omschrijving: {log.Description}");
                    Console.WriteLine($"Verdacht: {(log.Suspicious ? "Ja" : "Nee")}");
                    Console.WriteLine("---------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Kon logbestand niet lezen: {ex.Message}");
            }
        }

        Console.WriteLine("\nDruk op een toets om terug te keren naar het menu.");
        Console.ReadKey();
    }




}
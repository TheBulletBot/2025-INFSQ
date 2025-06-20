using System.Data.SQLite;

public class SuperAdmin : SystemAdmin
{
    public SuperAdmin(string username, string role) : base(username, role) { }

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
            "Add new System Administrator",
            "Update System Administrator",
            "Delete System Administrator",
            "Reset System Admin password",
            "Return"
        };

        int selection = 0;
        ConsoleKey key;

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Super Administrator Menu ({Username}) ===");

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
                    case 9: DeleteOwnAccountMenu(); break;
                    case 10: BackupMenu(); break;
                    case 11: RestoreBackup(); break;
                    case 12: ViewLogs(); break;
                    case 13: AddTravelerMenu(); break;
                    case 14: UpdateTravelerMenu(); break;
                    case 15: DeleteTravelerMenu(); break;
                    case 16: AddScooterMenu(); break;
                    case 17: UpdateScooterMenu(); break;
                    case 18: DeleteScooterMenu(); break;
                    case 19: SearchAndPrintTravelersMenu(); break;
                    case 21: AddSystemAdminMenu(); break;
                    case 22: UpdateSystemAdminMenu(); break;
                    case 23: DeleteSystemAdminMenu(); break;
                    case 24: ResetSystemAdminPasswordMenu(); break;
                    case 25: return;
                }
            }
        }
    }

    public void AddSystemAdmin(string username, string password, string firstName, string lastName)
    {
        string passwordHash = CryptographyHelper.CreateHashValue(password);
        string id = Guid.NewGuid().ToString();
        var date = DateTime.UtcNow.ToString("dd-MM-yyyy");

        var cmd = @"
            INSERT INTO User (Id, Username, PasswordHash, Role, FirstName, LastName, RegistrationDate)
            VALUES (@id, @username, @passwordHash, 'System Admin', @first, @last, @date)";
        var querycommand = new SQLiteCommand(cmd);
        querycommand.Parameters.AddWithValue("@id", id);
        querycommand.Parameters.AddWithValue("@username", CryptographyHelper.Encrypt(username));
        querycommand.Parameters.AddWithValue("@passwordHash", passwordHash);
        querycommand.Parameters.AddWithValue("@first", CryptographyHelper.Encrypt(firstName));
        querycommand.Parameters.AddWithValue("@last", CryptographyHelper.Encrypt(lastName));
        querycommand.Parameters.AddWithValue("@date", date);

        DatabaseHelper.ExecuteStatement(cmd);
        Logging.Log(Username, "Add System Admin", $"Nieuwe System Admin toegevoegd: {username}", false);
        Console.WriteLine("System Admin toegevoegd!");
    }

    public void UpdateSystemAdmin(string currentUsername, string newUsername, string newPassword, string firstName, string lastName)
    {
        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);

        var sql  = @"
            UPDATE User
            SET Username = @newUsername, PasswordHash = @passwordHash
            WHERE Username = @currentUsername AND Role = 'System Admin'";
        var cmd = new SQLiteCommand(sql);
        cmd.Parameters.AddWithValue("@newUsername", CryptographyHelper.Encrypt(newUsername));
        cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
        cmd.Parameters.AddWithValue("@currentUsername", CryptographyHelper.Encrypt(currentUsername));
        cmd.Parameters.AddWithValue("@currentUsername", CryptographyHelper.Encrypt(firstName));
        cmd.Parameters.AddWithValue("@currentUsername", CryptographyHelper.Encrypt(lastName));
        

        DatabaseHelper.ExecuteStatement(cmd);
        Logging.Log(Username, "Update System Admin", $"System Admin {currentUsername} gewijzigd naar {newUsername}", false);
        Console.WriteLine("System Admin bijgewerkt (of niet gevonden).");
    }

    public void DeleteSystemAdmin(string username)
    {
        var sql = @"
            DELETE FROM User
            WHERE Username = @username AND Role = 'System Admin'";
        var cmd = new SQLiteCommand(sql);
        cmd.Parameters.AddWithValue("@username", username);

        DatabaseHelper.ExecuteStatement(cmd);
        Logging.Log(Username, "Delete System Admin", $"System Admin verwijderd: {username}", true);
        Console.WriteLine($"System Admin verwijderd: {username}");
    }

    public void ResetSystemAdminPassword(string username)
    {
        string tempPassword = "Temp" + new Random().Next(1000, 9999);
        string passwordHash = CryptographyHelper.CreateHashValue(tempPassword);

        var sql =@"
            UPDATE User
            SET PasswordHash = @passwordHash
            WHERE Username = @username AND Role = 'System Admin'";
        var cmd = new SQLiteCommand(sql);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

        DatabaseHelper.ExecuteStatement(cmd);
        Logging.Log(Username, "Reset System Admin Password", $"Tijdelijk wachtwoord ingesteld voor: {username}", true);
        Console.WriteLine($"Tijdelijk wachtwoord voor {username}: {tempPassword}");
    }

    public void AddSystemAdminMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Voeg System Admin toe ===");
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

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Gebruikersnaam en wachtwoord mogen niet leeg zijn.");
            return;
        }

        AddSystemAdmin(username, password, firstName, lastName);
    }

    public void UpdateSystemAdminMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Pas System Admin aan ===");
        string currentUsername = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );
        string newUsername = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );

        string newPassword = Validation.ValidatedInput(
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

        if (string.IsNullOrWhiteSpace(currentUsername) || string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newPassword))
        {
            Console.WriteLine("Gebruikersnaam en wachtwoord mogen niet leeg zijn.");
            return;
        }

        UpdateSystemAdmin(currentUsername, newUsername, newPassword, firstName, lastName);
    }

    public void DeleteSystemAdminMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder System Admin ===");
        string username = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Gebruikersnaam mag niet leeg zijn.");
            return;
        }

        DeleteSystemAdmin(username);
    }

    public void ResetSystemAdminPasswordMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verander System Admin Wachtwoord ===");
        string username = Validation.ValidatedInput(
            Validation.UsernameRe,
            "Gebruikersnaam:",
            "Ongeldige gebruikersnaam. Deze moet 8–10 tekens zijn en mag alleen letters, cijfers, punten, apostrofs of underscores bevatten."
        );

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Gebruikersnaam mag niet leeg zijn.");
            return;
        }

        ResetSystemAdminPassword(username);
    }
}

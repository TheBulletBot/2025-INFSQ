public class SuperAdmin : SystemAdmin
{
    public SuperAdmin(string username, string role) : base(username, role) { }
    //Insert Menus for Admins here
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
                    //case 2: SearchScooter(); break;
                    case 3: ShowAllUsersAndRoles(); break;
                    case 4: AddEngineerMenu(); break;
                    case 5: UpdateEngineerMenu(); break;
                    case 6: DeleteEngineerMenu(); break;
                    case 7: ResetEngineerPasswordMenu(); break;
                    //case 8: UpdateOwnPasswordMenu(); break; ???
                    case 9: DeleteOwnAccountMenu(); break;
                    //case 10: BackupMenu(); break;
                    //case 11: RestoreSystem(); break;
                    //case 12: ViewLogs(); break;
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

    public void AddSystemAdminMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Voeg System Admin toe ===");
        Console.Write("Gebruikersnaam: ");
        string username = Console.ReadLine();
        Console.Write("Wachtwoord: ");

        string password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Gebruikersnaam en wachtwoord mogen niet leeg zijn.");
            return;
        }

        AddSystemAdmin(username, password);
    }

    public void AddSystemAdmin(string username, string password)
    {
        string passwordHash = CryptographyHelper.CreateHashValue(password);
        string id = Guid.NewGuid().ToString();

        string sql = $@"
            INSERT INTO User (Id, Username, PasswordHash, Role)
            VALUES ('{id}', '{username}', '{passwordHash}', 'System Admin')
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "Add System Admin", $"Nieuwe System Admin toegevoegd: {username}", false);
        Console.WriteLine("System Admin toegevoegd!");
    }

    public void UpdateSystemAdminMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Pas System Admin aan ===");
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

        UpdateSystemAdmin(currentUsername, newUsername, newPassword);
    }

    public void UpdateSystemAdmin(string currentUsername, string newUsername, string newPassword)
    {
        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);

        string sql = $@"
            UPDATE User
            SET Username = '{newUsername}', PasswordHash = '{passwordHash}'
            WHERE Username = '{currentUsername}' AND Role = 'System Admin'
        ";
        
        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "Update System Admin", $"System Admin {currentUsername} gewijzigd naar {newUsername}", false);
        Console.WriteLine("System Admin bijgewerkt (of niet gevonden).");
    }

    public void DeleteSystemAdminMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder System Admin ===");
        Console.Write("Gebruikersnaam: ");
        string username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Gebruikersnaam mag niet leeg zijn.");
            return;
        }

        DeleteSystemAdmin(username);
    }

    public void DeleteSystemAdmin(string username)
    {
        string sql = $@"
            DELETE FROM User
            WHERE Username = '{username}' AND Role = 'System Admin'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "Delete System Admin", $"System Admin verwijderd: {username}", true);
        Console.WriteLine($"System Admin verwijderd: {username}");
    }

    public void ResetSystemAdminPasswordMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Verwijder System Admin ===");
        Console.Write("Gebruikersnaam: ");
        string username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Gebruikersnaam mag niet leeg zijn.");
            return;
        }

        ResetSystemAdminPassword(username);
    }

    public void ResetSystemAdminPassword(string username)
    {
        string tempPassword = "Temp" + new Random().Next(1000, 9999);
        string passwordHash = CryptographyHelper.CreateHashValue(tempPassword);

        string sql = $@"
            UPDATE User
            SET PasswordHash = '{passwordHash}'
            WHERE Username = '{username}' AND Role = 'System Admin'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(Username, "Reset System Admin Password", $"Tijdelijk wachtwoord ingesteld voor: {username}", true);
        Console.WriteLine($"Tijdelijk wachtwoord voor {username}: {tempPassword}");
    }
    

}

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
        "Add new Traveller",
        "Update Traveller info",
        "Delete Traveller",
        "Add new Scooter",
        "Update Scooter info",
        "Delete Scooter",
        "Search Traveller",
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
            switch (selection)
            {
                case 0: ChangePassword(); break;
                case 1: ChangeScooter(); break;
                case 2: SearchScooter(); break;
                case 3: ListUsersAndRoles(); break;
                case 4: AddServiceEngineer(); break;
                case 5: UpdateServiceEngineer(); break;
                case 6: DeleteServiceEngineer(); break;
                case 7: ResetServiceEngineerPassword(); break;
                case 8: UpdateMyProfile(); break;
                case 9: DeleteMyAccount(); break;
                case 10: BackupSystem(); break;
                case 11: RestoreSystem(); break;
                case 12: ViewLogs(); break;
                case 13: AddTraveller(); break;
                case 14: UpdateTraveller(); break;
                case 15: DeleteTraveller(); break;
                case 16: AddScooter(); break;
                case 17: UpdateScooter(); break;
                case 18: DeleteScooter(); break;
                case 19: SearchTraveller(); break;
                case 20: return;
            }
        }
    }
}
    

    private void ListUsersAndRoles()
    {
        var users = DatabaseHelper.Query<User>("SELECT Username, Role FROM Users");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Username} - {user.Role}");
        }
    }

    private void AddServiceEngineer()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();
        string sql = $"INSERT INTO Users (Username, Password, Role) VALUES ('{username}', '{password}', 'Service Engineer')";
        DatabaseHelper.ExecuteStatement(sql);
    }

    private void UpdateServiceEngineer()
    {
        Console.Write("Enter username to update: ");
        string username = Console.ReadLine();
        Console.Write("New password: ");
        string password = Console.ReadLine();
        string sql = $"UPDATE Users SET Password = '{password}' WHERE Username = '{username}' AND Role = 'Service Engineer'";
        DatabaseHelper.ExecuteStatement(sql);
    }

    private void DeleteServiceEngineer()
    {
        Console.Write("Enter username to delete: ");
        string username = Console.ReadLine();
        string sql = $"DELETE FROM Users WHERE Username = '{username}' AND Role = 'Service Engineer'";
        DatabaseHelper.ExecuteStatement(sql);
    }

    private void ResetServiceEngineerPassword()
    {
        Console.Write("Enter username to reset password: ");
        string username = Console.ReadLine();
        string tempPassword = "Temp1234"; // Random generator is beter in productie
        string sql = $"UPDATE Users SET Password = '{tempPassword}' WHERE Username = '{username}' AND Role = 'Service Engineer'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine($"Temporary password set: {tempPassword}");
    }

    private void UpdateMyProfile()
    {
        Console.Write("New password: ");
        string newPass = Console.ReadLine();
        string sql = $"UPDATE Users SET Password = '{newPass}' WHERE Username = '{Username}'";
        DatabaseHelper.ExecuteStatement(sql);
    }

    private void DeleteMyAccount()
    {
        string sql = $"DELETE FROM Users WHERE Username = '{Username}'";
        DatabaseHelper.ExecuteStatement(sql);
    }

    private void BackupSystem()
    {
        string backupName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.db";
        File.Copy("INFSQScooterBackend.db", backupName);
        Console.WriteLine($"Backup created: {backupName}");
    }

    private void RestoreSystem()
    {
        Console.Write("Enter one-time backup restore code: ");
        string code = Console.ReadLine();

        // Simuleer validatie
        if (code == "RESTORE123") // In praktijk valideren via een aparte tabel
        {
            Console.Write("Enter backup file path: ");
            string backupPath = Console.ReadLine();
            if (File.Exists(backupPath))
            {
                File.Copy(backupPath, "INFSQScooterBackend.db", overwrite: true);
                Console.WriteLine("Backup restored successfully.");
            }
            else
            {
                Console.WriteLine("Backup file not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid code.");
        }
    }

    private void ViewLogs()
    {
        string path = "logfile.txt";
        if (File.Exists(path))
        {
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
                Console.WriteLine(line);
        }
        else
        {
            Console.WriteLine("No log file found.");
        }
    }

    private void AddTraveller()
    {
        Console.WriteLine("=== Add New Traveler ===");

        Console.Write("First name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last name: ");
        string lastName = Console.ReadLine();
        Console.Write("Birthday (yyyy-MM-dd): ");
        string birthday = Console.ReadLine();
        Console.Write("Gender: ");
        string gender = Console.ReadLine();
        Console.Write("Street name: ");
        string street = Console.ReadLine();
        Console.Write("House number: ");
        string houseNumber = Console.ReadLine();
        Console.Write("Zip code: ");
        string zip = Console.ReadLine();
        Console.Write("City: ");
        string city = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Phone: ");
        string phone = Console.ReadLine();
        Console.Write("License number: ");
        string license = Console.ReadLine();

        string sql = $@"
        INSERT INTO Travelers 
        (FirstName, LastName, Birthday, Gender, StreetName, HouseNumber, ZipCode, City, Email, Phone, LicenseNumber)
        VALUES 
        ('{firstName}', '{lastName}', '{birthday}', '{gender}', '{street}', '{houseNumber}', '{zip}', '{city}', '{email}', '{phone}', '{license}')
    ";

        DatabaseHelper.ExecuteStatement(sql);
    }

    private void UpdateTraveller()
    {
        Console.Write("Enter Traveler ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        Console.Write("New first name: ");
        string firstName = Console.ReadLine();
        Console.Write("New last name: ");
        string lastName = Console.ReadLine();
        Console.Write("New birthday (yyyy-MM-dd): ");
        string birthday = Console.ReadLine();
        Console.Write("New gender: ");
        string gender = Console.ReadLine();
        Console.Write("New street name: ");
        string street = Console.ReadLine();
        Console.Write("New house number: ");
        string houseNumber = Console.ReadLine();
        Console.Write("New zip code: ");
        string zip = Console.ReadLine();
        Console.Write("New city: ");
        string city = Console.ReadLine();
        Console.Write("New email: ");
        string email = Console.ReadLine();
        Console.Write("New phone: ");
        string phone = Console.ReadLine();
        Console.Write("New license number: ");
        string license = Console.ReadLine();

        string sql = $@"
        UPDATE Travelers SET 
            FirstName = '{firstName}',
            LastName = '{lastName}',
            Birthday = '{birthday}',
            Gender = '{gender}',
            StreetName = '{street}',
            HouseNumber = '{houseNumber}',
            ZipCode = '{zip}',
            City = '{city}',
            Email = '{email}',
            Phone = '{phone}',
            LicenseNumber = '{license}'
        WHERE Id = {id}
    ";

        DatabaseHelper.ExecuteStatement(sql);
    }

    private void DeleteTraveller()
    {
        Console.Write("Enter Traveler ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        string sql = $"DELETE FROM Travelers WHERE Id = {id}";
        DatabaseHelper.ExecuteStatement(sql);
    }

    private void AddScooter()
    {
        Console.Write("Serial number: ");
        string serial = Console.ReadLine();
        Console.Write("Brand: ");
        string brand = Console.ReadLine();
        string sql = $"INSERT INTO Scooters (SerialNumber, Brand) VALUES ('{serial}', '{brand}')";
        DatabaseHelper.ExecuteStatement(sql);
    }

    private void UpdateScooter()
    {
        Console.Write("Serial number to update: ");
        string serial = Console.ReadLine();
        Console.Write("New brand: ");
        string brand = Console.ReadLine();
        string sql = $"UPDATE Scooters SET Brand = '{brand}' WHERE SerialNumber = '{serial}'";
        DatabaseHelper.ExecuteStatement(sql);
    }

    private void DeleteScooter()
    {
        Console.Write("Enter serial number to delete: ");
        string serial = Console.ReadLine();
        string sql = $"DELETE FROM Scooters WHERE SerialNumber = '{serial}'";
        DatabaseHelper.ExecuteStatement(sql);
    }

    private void SearchTraveller()
    {
        
    }
}
class Program
{
    static void Main(string[] args)
    {
        var system = new SystemADFunc();

        Console.WriteLine("=== Inlogscherm ===");
        Console.Write("Gebruikersnaam: ");
        string username = Console.ReadLine();

        Console.Write("Wachtwoord: ");
        string password = ReadPassword();

        string role = system.Login(username, password);

        if (role == null)
        {
            Console.WriteLine("\n❌ Ongeldige gebruikersnaam of wachtwoord.");
            return;
        }

        Console.WriteLine($"\n✅ Ingelogd als {username} ({role})");

        while (true)
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("1. Wachtwoord wijzigen");
            Console.WriteLine("2. Account verwijderen");
            Console.WriteLine("3. Zoek Traveller");
            Console.WriteLine("4. Scooter toevoegen");

            if (role == "Admin") // Extra opties alleen voor admin
            {
                Console.WriteLine("5. Engineer toevoegen");
                Console.WriteLine("6. Engineer updaten");
                Console.WriteLine("7. Engineer verwijderen");
                Console.WriteLine("8. Engineer wachtwoord resetten");
                Console.WriteLine("9. Toon gebruikers");
            }

            Console.WriteLine("0. Afsluiten");
            Console.Write("Keuze: ");
            string keuze = Console.ReadLine();

            switch (keuze)
            {
                case "1":
                    Console.Write("Nieuw wachtwoord: ");
                    string nieuwWW = ReadPassword();
                    system.UpdateOwnPassword(username, nieuwWW);
                    break;

                case "2":
                    system.DeleteOwnAccount(username);
                    Console.WriteLine("Account verwijderd. Tot ziens!");
                    return;

                case "3":
                Console.Write("Zoekterm: ");
                string zoek = Console.ReadLine();
                system.SearchAndPrintTravellers(zoek); // nieuwe methode aanroepen
                break;


                case "4":
                    Console.Write("Merk: "); var merk = Console.ReadLine();
                    Console.Write("Model: "); var model = Console.ReadLine();
                    Console.Write("TopSpeed: "); var top = int.Parse(Console.ReadLine());
                    Console.Write("Battery: "); var bat = int.Parse(Console.ReadLine());
                    Console.Write("Charge: "); var ch = int.Parse(Console.ReadLine());
                    Console.Write("Range: "); var rng = int.Parse(Console.ReadLine());
                    Console.Write("Locatie: "); var loc = Console.ReadLine();
                    Console.Write("Out of Service (0/1): "); var oos = int.Parse(Console.ReadLine());
                    Console.Write("Mileage: "); var km = int.Parse(Console.ReadLine());
                    Console.Write("Onderhoudsdatum (yyyy-MM-dd): ");
                    var date = DateTime.Parse(Console.ReadLine());

                    system.AddScooter(merk, model, top, bat, ch, rng, loc, oos, km, date);
                    Console.WriteLine("Scooter toegevoegd.");
                    break;

                case "5" when role == "Admin":
                    Console.Write("Gebruikersnaam: ");
                    var u = Console.ReadLine();
                    Console.Write("Wachtwoord: ");
                    var p = ReadPassword();
                    system.AddEngineer(u, p);
                    break;

                case "6" when role == "Admin":
                    Console.Write("Huidige naam: ");
                    var oud = Console.ReadLine();
                    Console.Write("Nieuwe naam: ");
                    var nieuw = Console.ReadLine();
                    Console.Write("Nieuw wachtwoord: ");
                    var nieuwPass = ReadPassword();
                    system.UpdateEngineer(oud, nieuw, nieuwPass);
                    break;

                case "7" when role == "Admin":
                    Console.Write("Gebruiker verwijderen: ");
                    var del = Console.ReadLine();
                    system.DeleteEngineer(del);
                    break;

                case "8" when role == "Admin":
                    Console.Write("Engineer naam: ");
                    var reset = Console.ReadLine();
                    system.ResetEngineerPassword(reset);
                    break;

                case "9" when role == "Admin":
                    system.ShowAllUsersAndRoles();
                    break;

                case "0":
                    Console.WriteLine("Tot ziens!");
                    return;

                default:
                    Console.WriteLine("❌ Ongeldige keuze.");
                    break;
            }
        }
    }

    static string ReadPassword()
    {
        var pwd = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && pwd.Length > 0)
            {
                Console.Write("\b \b");
                pwd = pwd[..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                pwd += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);
        Console.WriteLine();
        return pwd;
    }
}

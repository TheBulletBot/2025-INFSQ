using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


public class ServiceEngineer : User
{
    // aanpassen tot de database maar ik heb geen idee (:
    public ServiceEngineer(string username, string role) : base(username, role)
    {

    }

    public override void Menu()
    {
        string[] options = {
        "Change password",
        "Edit scooter attributes",
        "Search scooter",
        "Return"
    };

        int selection = 0;
        ConsoleKey key;

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Service Engineer Menu ({Username}) ===");

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
                    case 0:
                        UpdateOwnPasswordMenu();
                        //UpdateOwnPassword();
                        break;
                    case 1:
                        UpdateScooterMenu();
                        //UpdateScooter();
                        break;
                    case 2:
                        SearchScooterMenu(); 
                        break;
                    case 3:
                        return;
                }
            }
        }
    }

    public void UpdateScooterMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Scooter Bijwerken ===");

        Console.Write("Voer het ID van de scooter in die je wilt bijwerken: ");
        string id = Console.ReadLine();

        Console.Write("Nieuw merk: ");
        string brand = Console.ReadLine();

        Console.Write("Nieuw model: ");
        string model = Console.ReadLine();

        Console.Write("Nieuwe topsnelheid (km/u): ");
        int topSpeed = int.Parse(Console.ReadLine());

        Console.Write("Nieuwe batterijcapaciteit (Wh): ");
        int battery = int.Parse(Console.ReadLine());

        Console.Write("Nieuwe ladingstatus (%): ");
        int charge = int.Parse(Console.ReadLine());

        Console.Write("Nieuw totaal bereik (km): ");
        int totalRange = int.Parse(Console.ReadLine());

        Console.Write("Nieuwe locatie: ");
        string location = Console.ReadLine();

        Console.Write("Buiten dienst? (1 = ja, 0 = nee): ");
        int outOfService = int.Parse(Console.ReadLine());

        Console.Write("Nieuwe kilometerstand (km): ");
        int mileage = int.Parse(Console.ReadLine());

        Console.Write("Datum laatste onderhoud (YYYY-MM-DD): ");
        DateTime lastMaintenance;
        while (!DateTime.TryParse(Console.ReadLine(), out lastMaintenance))
        {
            Console.Write("Ongeldige datum. Probeer opnieuw (YYYY-MM-DD): ");
        }

        UpdateScooter(id, brand, model, topSpeed, battery, charge, totalRange, location, outOfService, mileage, lastMaintenance);

        Console.WriteLine("\nDruk op een toets om terug te keren naar het menu");
        Console.ReadKey();
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
        Logging.Log(this.Username, "Update Scooter", $"Scooter bijgewerkt met ID: {id}", false);
        Console.WriteLine("Scooter succesvol bijgewerkt.");
    }
    public void UpdateOwnPasswordMenu()
    {
        bool isPasswordValid = false;
        while (!isPasswordValid)
        {
            Console.Clear();
            Console.WriteLine("=== Change Password ===");
            Console.Write("Enter your new password: ");
            string newPassword = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                Console.WriteLine("Password cannot be empty. Please try again.");
                continue; //continue brings you to the next iteration in the loop
            }
            else if (Regex.IsMatch(newPassword, Validation.PasswordRe))
            {
                UpdateOwnPassword(newPassword);
                isPasswordValid = true;
                break; //break exits the loop
            }
        }

    }
    public void UpdateOwnPassword(string newPassword)
    {
        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);
        string sql = $"UPDATE User SET PasswordHash = '{passwordHash}' WHERE Username = '{this.Username}'";
        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(this.Username, "Change Password", "Gebruiker heeft zijn wachtwoord gewijzigd", true);
        Console.WriteLine("Je wachtwoord is succesvol gewijzigd.");
    }

    public void SearchScooterMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Zoek Scooter ===");
        Console.Write("Voer een zoekterm in (bijv. merk, model, locatie...): ");
        string keyword = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            Console.WriteLine("Zoekterm mag niet leeg zijn.");
            return;
        }

        SearchScooter(keyword);

        Console.WriteLine("\nDruk op een toets om terug te keren naar het menu.");
        Console.ReadKey();
    }



    public void SearchScooter(string keyword)
    {
        string sql = $@"
        SELECT * FROM Scooter
        WHERE 
            Id LIKE '%{keyword}%' OR
            Brand LIKE '%{keyword}%' OR
            Model LIKE '%{keyword}%' OR
            Location LIKE '%{keyword}%' OR
            SerialNumber LIKE '%{keyword}%' OR
            LastMaintenance LIKE '%{keyword}%' OR
        ";
        var scooters = DatabaseHelper.Query<Scooter>(sql);

        Console.WriteLine("\n--- Zoekresultaten voor scooters ---\n");

        if (scooters.Count == 0)
        {
            Console.WriteLine("Geen scooters gevonden die overeenkomen met de zoekterm.");
            return;
        }

        foreach (var s in scooters)
        {
            Console.WriteLine(
                $"ID: {s.SerialNumber}\nMerk: {s.Brand}\nModel: {s.Model}\nSnelheid: {s.TopSpeed} km/u\n" +
                $"Batterij: {s.BatteryCapacity} Wh\nLading: {s.StateOfCharge}%\nLocatie: {s.Location}\n" +
                $"Onderhoud: {s.LastMaintenance:yyyy-MM-dd}\n--------------------------");
        }
        Logging.Log(this.Username, "Search Scooter", $"Scooter gezocht met keyword: {keyword}", false);
    }

}
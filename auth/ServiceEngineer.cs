using System.Data.SQLite;
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
                    //SearchScooter();
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

        string id = Validation.ValidatedInput(
            Validation.IdRe,
            "Voer het ID van de scooter in die je wilt bijwerken:",
            "Ongeldig ID. Gebruik alleen cijfers."
        );

        string brand = Validation.ValidatedInput(
            Validation.BrandRe,
            "Nieuw merk:",
            "Ongeldig merk. Gebruik 2–20 letters, cijfers, spaties of streepjes."
        );

        string model = Validation.ValidatedInput(
            Validation.ModelRe,
            "Nieuw model:",
            "Ongeldig model. Gebruik 1–20 letters, cijfers, spaties of streepjes."
        );

        Console.Write("Nieuwe topsnelheid (km/u): ");
        int topSpeed;
        while (!int.TryParse(Console.ReadLine(), out topSpeed))
        {
            Console.Write("Ongeldige invoer. Voer een geldig getal in voor de topsnelheid: ");
        }

        Console.Write("Nieuwe batterijcapaciteit (Wh): ");
        int battery;
        while (!int.TryParse(Console.ReadLine(), out battery))
        {
            Console.Write("Ongeldige invoer. Voer een geldig getal in voor de batterijcapaciteit: ");
        }

        Console.Write("Nieuwe ladingstatus (%): ");
        int charge;
        while (!int.TryParse(Console.ReadLine(), out charge))
        {
            Console.Write("Ongeldige invoer. Voer een geldig percentage in voor de ladingstatus: ");
        }

        Console.Write("Nieuw totaal bereik (km): ");
        int totalRange;
        while (!int.TryParse(Console.ReadLine(), out totalRange))
        {
            Console.Write("Ongeldige invoer. Voer een geldig getal in voor het bereik: ");
        }

        string location = Validation.ValidatedInput(
            Validation.LocationRe,
            "Nieuwe locatie:",
            "Ongeldige locatie. Gebruik 2–30 tekens, letters/cijfers/spaties/komma’s/punten/streepjes."
        );

        Console.Write("Buiten dienst? (1 = ja, 0 = nee): ");
        int outOfService;
        while (!int.TryParse(Console.ReadLine(), out outOfService) || (outOfService != 0 && outOfService != 1))
        {
            Console.Write("Ongeldige invoer. Voer 1 in voor ja of 0 voor nee: ");
        }

        Console.Write("Nieuwe kilometerstand (km): ");
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

        UpdateScooter(id, brand, model, topSpeed, battery, charge, totalRange, location, outOfService, mileage, lastMaintenance);

        Console.WriteLine("\nDruk op een toets om terug te keren naar het menu.");
        Console.ReadKey();
    }
    public void UpdateScooter(string id, string brand, string model, int topSpeed, int battery, int charge, int totalRange, string location, int outOfService, int mileage, DateTime lastMaintenance)
    {
        string formattedDate = lastMaintenance.ToString("yyyy-MM-dd");

        string sql = $@"
            UPDATE Scooter
            SET Brand = '@brand',
                Model = '@model',
                TopSpeed = @topspeed,
                BatteryCapacity = @battery,
                StateOfCharge = @charge,
                TargetRange = @totalrange,
                Location = '@location',
                OutOfService = @outofService,
                Mileage = @mileage,
                LastMaintenance = '@lastMaintenance'
            WHERE Id = '@id'
        ";
        var queryCommand = new SQLiteCommand(sql);
            queryCommand.Parameters.Add(new SQLiteParameter("@brand", brand));
            queryCommand.Parameters.Add(new SQLiteParameter("@model", model));
            queryCommand.Parameters.Add(new SQLiteParameter("@topspeed", topSpeed));
            queryCommand.Parameters.Add(new SQLiteParameter("@battery", battery));
            queryCommand.Parameters.Add(new SQLiteParameter("@charge", charge));
            queryCommand.Parameters.Add(new SQLiteParameter("@totalrange", totalRange));
            queryCommand.Parameters.Add(new SQLiteParameter("@location", location));
            queryCommand.Parameters.Add(new SQLiteParameter("@outofService", outOfService));
            queryCommand.Parameters.Add(new SQLiteParameter("@mileage", mileage));
            queryCommand.Parameters.Add(new SQLiteParameter("@lastMaintenance", brand));
            queryCommand.Parameters.Add(new SQLiteParameter("@id", id));



        DatabaseHelper.ExecuteStatement(queryCommand);
        Logging.Log(this.Username, "Update Scooter", $"Updated Scooter with ID: {id}", false);
        Console.WriteLine("Scooter succesvol bijgewerkt.");
    }
    public void UpdateOwnPasswordMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Wachtwoord wijzigen ===");

        string newPassword = Validation.ValidatedInput(
            Validation.PasswordRe,
            "Voer uw nieuwe wachtwoord in:",
            "Ongeldig wachtwoord. Het wachtwoord moet 12–30 tekens lang zijn en minstens één kleine letter, één hoofdletter, één cijfer en één speciaal teken bevatten."
        );

        UpdateOwnPassword(Username, newPassword);
    }
    public void UpdateOwnPassword(string username, string newPassword) //(string OwnUserName)
    {
        // kan nu nog alle ww veranderen 
        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);
        string sql = $"UPDATE User SET PasswordHash = '{passwordHash}' WHERE Username = '{username}'";
        DatabaseHelper.ExecuteStatement(sql);
        Logging.Log(this.Username, "Update Own Password", $"Changed own password.", false);
        Console.WriteLine("Je account is bijgewerkt.");
    }
}
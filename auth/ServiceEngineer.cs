using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


public class ServiceEngineer : User
{
    // aanpassen tot de database maar ik heb geen idee (:
    private readonly context dbContext;
    private readonly List<Scooter> scooters;
    public ServiceEngineer(string username, string role) : base(username, role)
    {
     
    }

    public override void Menu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Service Engineer Menu ({Username}) ===");
            Console.WriteLine("1. Change password");
            Console.WriteLine("2. Edit scooter attributes");
            Console.WriteLine("3. Search scooter");
            Console.WriteLine("4. Return");

            Console.Write("Pick an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ChangePassword();
                    break;
                case "2":
                    ChangeScooter();
                    break;
                case "3":
                    SearchScooter();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ChangePassword()
    {
        Console.Write("Enter your new password: ");
        string password = Console.ReadLine();
        Console.WriteLine("Password succesfully changed.");
        Console.WriteLine("Hit any key to continue");
        Console.ReadKey();
    }

    private void ChangeScooter()
    {
        Console.Write("Fill in the serial number: ");
        string serial = Console.ReadLine();

        var scooter = dbContext.Scooters.FirstOrDefault(s => s.SerialNumber == serial);

        if (scooter == null)
        {
            Console.WriteLine("Scooter not found");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nCurrent location: {scooter.Location}");
        Console.Write("New location (Enter to keep): ");
        string location = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(location))
            scooter.Location = location;

        Console.WriteLine($"\nCurrent charge state: {scooter.StateOfCharge}%");
        Console.Write("New charge state: ");
        if (int.TryParse(Console.ReadLine(), out int soc))
            scooter.StateOfCharge = soc;

        Console.WriteLine($"\nCurrent Mileage: {scooter.Mileage} km");
        Console.Write("New mileage: ");
        if (float.TryParse(Console.ReadLine(), out float mileage))
            scooter.Mileage = mileage;

        Console.WriteLine("\nChanges saved");
        dbContext.SaveChanges();
        Console.ReadKey();
    }

    private void SearchScooter()
    {
        Console.Write("Fill in search term: ");
        string searchterm = Console.ReadLine()?.ToLower();

        var results = scooters.Where(s =>
            s.SerialNumber.Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
            s.Brand.Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
            s.Model.Contains(searchterm, StringComparison.OrdinalIgnoreCase) ||
            s.Location.Contains(searchterm, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        Console.WriteLine("\n--- Results ---");
        if (results.Any())
        {
            foreach (var s in results)
            {
                Console.WriteLine($"Serial: {s.SerialNumber}, Brand: {s.Brand}, Model: {s.Model}, Location: {s.Location}");
            }
        }
        else
        {
            Console.WriteLine("No scooters found");
        }

        Console.WriteLine("Hit any key to return");
        Console.ReadKey();
    }
}
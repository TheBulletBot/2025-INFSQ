using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


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
                    //UpdateOwnPassword();
                    break;
                case 1:
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
        Console.WriteLine("Scooter succesvol bijgewerkt.");
    }
    public void UpdateOwnPassword(string username, string newPassword) //(string OwnUserName)
    {
        // kan nu nog alle ww veranderen 
        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);
        string sql = $"UPDATE User SET PasswordHash = '{passwordHash}' WHERE Username = '{username}'";
        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("Je account is bijgewerkt.");
    }
}
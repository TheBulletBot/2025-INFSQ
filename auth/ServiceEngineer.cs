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
                    ChangePassword();
                    break;
                case 1:
                    ChangeScooter();
                    break;
                case 2:
                    SearchScooter();
                    break;
                case 3:
                    return;
            }
        }
    }
}

    protected void ChangePassword()
    {
        Console.Write("Enter your new password: ");
        string password = Console.ReadLine();
        Console.WriteLine("Password succesfully changed.");
        Console.WriteLine("Hit any key to continue");
        Console.ReadKey();
    }

    protected void ChangeScooter()
    {
        
    }

    protected void SearchScooter()
    {
        
    }
}
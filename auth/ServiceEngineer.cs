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
        
    }

    private void SearchScooter()
    {
        
    }
}
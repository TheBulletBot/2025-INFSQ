using System.Collections;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;

public static class Login
{
    public static User user;
    public static void LoginScreen()
    {
        System.Console.WriteLine("Welcome to ScooterVille's Scooter Rental Management System\n \nPlease Log in.\n\n\n");
        System.Console.WriteLine("Username: ");
        var username = Console.ReadLine();
        System.Console.WriteLine("\nPassword: ");

        //compute hash
        var password = Console.ReadLine();

        System.Console.WriteLine($"DEBUG: Username = {username}; PasswordHash = {password}");

        //fetch User with username = 
        var baseQueryString = @"SELECT * FROM User u WHERE u.Username = @username";
        var queryCommand = new SqliteCommand(baseQueryString);
        queryCommand.Parameters.Add(new SqliteParameter("@username", username));
        var loginAttemptDBUser = DatabaseHelper.Query<DBUser>(queryCommand);
        if (loginAttemptDBUser.Count > 0)
        {
            //Return invalid username Error
            System.Console.WriteLine(loginAttemptDBUser[0].Username);
        }
        //compare passwordHash with Hashed password input. 
        /*if (password == loginAttemptDBUser[0].PasswordHash)
        {
            //Role Check
            switch (loginAttemptDBUser[0].Role)
            {
                case "ENGINEER":
                    user = new ServiceEngineer(loginAttemptDBUser[0].Username, loginAttemptDBUser[0].Role); break;
                case "ADMIN":
                    user = new Admin(loginAttemptDBUser[0].Username, loginAttemptDBUser[0].Role); break;
                case "SUPERADMIN":
                    user = new SuperAdmin(loginAttemptDBUser[0].Username, loginAttemptDBUser[0].Role); break;
            }
            user.Menu();
        }*/

    }
}
using System.Collections;
using System.Data.SQLite;
using System.Security.Cryptography;

public static class Login
{
    public static User user;
    public static void LoginScreen()
    {
        System.Console.WriteLine("Welkom bij ScooterVille's Scooterverhuur Management System\n \nLog in alstublieft.\n\n\n");
        int attempts = 0;
        bool correctLogin = false;
        while (!correctLogin)
        {
            if (attempts > 0)
            {
                Console.Clear();
                System.Console.WriteLine("----- Onjuiste Gebruikersnaam of wachtwoord, Probeer opnieuw----- \n\n");
            }
            if (attempts > 3)
            {
                Console.Clear();
                System.Console.WriteLine("---- Login limiet bereikt. De applicatie sluit nu. ----");
                return;
            }
            System.Console.WriteLine("Gebruikersnaam: ");//DEBUG
            var username = CryptographyHelper.Encrypt(Console.ReadLine());
            System.Console.WriteLine("\nWachtwoord: ");//DEBUG

            //compute hash
            var password = CryptographyHelper.CreateHashValue(Console.ReadLine());

            if (username == CryptographyHelper.Encrypt("super_admin") && password == CryptographyHelper.CreateHashValue("Admin_123?"))
            {
                user = new SuperAdmin(CryptographyHelper.Decrypt(username), "SUPERADMIN");
                Logging.Log(user.Username, "Login", "Successful Login", false);
                user.Menu();
                correctLogin = true;

                break;
            }
            else
            {
                System.Console.WriteLine($"DEBUG: Username = {username}; PasswordHash = {password}");//DEBUG

                //fetch User with username = 
                var baseQueryString = @"SELECT * FROM User u WHERE u.Username = @username";
                var queryCommand = new SQLiteCommand(baseQueryString);
                queryCommand.Parameters.Add(new SQLiteParameter("@username", username));
                var loginAttemptDBUser = DatabaseHelper.Query<DBUser>(queryCommand);
                if (loginAttemptDBUser.Count <= 0)
                {
                    //Return invalid username Error
                    System.Console.WriteLine(CryptographyHelper.Decrypt(loginAttemptDBUser[0].Username));//DEBUG

                    Logging.Log(user.Username, "Login", "Unsuccessful login: Wrong username", attempts>1);
                    continue;
                }
                //compare passwordHash with Hashed password input. 
                if (password == loginAttemptDBUser[0].PasswordHash)
                {
                    //Role Check
                    switch (loginAttemptDBUser[0].Role)
                    {
                        case "ENGINEER":
                            user = new ServiceEngineer(CryptographyHelper.Decrypt(loginAttemptDBUser[0].Username), loginAttemptDBUser[0].Role); break;
                        case "ADMIN":
                            user = new SystemAdmin(CryptographyHelper.Decrypt(loginAttemptDBUser[0].Username), loginAttemptDBUser[0].Role); break;
                        case "SUPERADMIN":
                            user = new SuperAdmin(CryptographyHelper.Decrypt(loginAttemptDBUser[0].Username), loginAttemptDBUser[0].Role); break;
                    }
                    Logging.Log(user.Username, "Login", "Successful Login", false);
                    user.Menu();
                    correctLogin = true;
                    break;
                }
                else
                {
                    Logging.Log(user.Username, "Login", "Unsuccessful login: Wrong Password", attempts>1);
                }
            }
        }

    }
}
using System.Collections;
using System.Data.SQLite;
using System.Security.Cryptography;

public static class Login
{
    public static User user;
    public static void LoginScreen()
    {
        Console.WriteLine("Welkom bij ScooterVille's Scooterverhuur Management System\n \nLog in alstublieft.\n\n\n");
        int attempts = 0;
        bool correctLogin = false;

        while (!correctLogin)
        {
            if (attempts > 0)
            {
                Console.Clear();
                Console.WriteLine("----- Onjuiste Gebruikersnaam of wachtwoord, Probeer opnieuw ----- \n\n");
            }
            if (attempts >= 3)
            {
                Console.Clear();
                Console.WriteLine("---- Login limiet bereikt. De applicatie sluit nu. ----");
                return;
            }

            Console.Write("Gebruikersnaam: ");
            var usernameEncrypted = CryptographyHelper.Encrypt(Console.ReadLine());

            Console.Write("\nWachtwoord: ");
            var passwordHash = CryptographyHelper.CreateHashValue(Console.ReadLine());

            // Speciale hardcoded login voor super_admin
            if (usernameEncrypted == CryptographyHelper.Encrypt("super_admin") && passwordHash == CryptographyHelper.CreateHashValue("Admin_123?"))
            {
                user = new SuperAdmin("super_admin", "SUPERADMIN");
                Logging.Log(user.Username, "Login", "Successful Login", false);
                user.Menu();
                correctLogin = true;
                break;
            }

            // Probeer gebruiker op te halen uit database
            var query = @"SELECT * FROM User u WHERE u.Username = @username";
            var cmd = new SqliteCommand(query);
            cmd.Parameters.Add(new SqliteParameter("@username", usernameEncrypted));
            var result = DatabaseHelper.Query<DBUser>(cmd);

            if (result.Count <= 0)
            {
                string usernameDecrypted = CryptographyHelper.Decrypt(usernameEncrypted);
                Logging.Log(usernameDecrypted, "Login", "Unsuccessful login: Wrong username", attempts > 1);
                attempts++;
                continue;
            }

            var dbUser = result[0];

            if (passwordHash == dbUser.PasswordHash)
            {
                string decryptedUsername = CryptographyHelper.Decrypt(dbUser.Username);

                switch (dbUser.Role.ToUpper())
                {
                    case "ENGINEER":
                        user = new ServiceEngineer(decryptedUsername, dbUser.Role); break;
                    case "ADMIN":
                        user = new SystemAdmin(decryptedUsername, dbUser.Role); break;
                    case "SUPERADMIN":
                        user = new SuperAdmin(decryptedUsername, dbUser.Role); break;
                    default:
                        Console.WriteLine("Onbekende rol. Toegang geweigerd.");
                        return;
                }

                Logging.Log(user.Username, "Login", "Successful Login", false);
                user.Menu();
                correctLogin = true;
                break;
            }
            else
            {
                string usernameDecrypted = CryptographyHelper.Decrypt(dbUser.Username);
                Logging.Log(usernameDecrypted, "Login", "Unsuccessful login: Wrong Password", attempts > 1);
                attempts++;
            }
        }
    }
}

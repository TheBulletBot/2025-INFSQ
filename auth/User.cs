using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


public abstract class User
{
    public string Username { get; }
    public string Role { get; }

    public User(string username, string role)
    {
        this.Username = username;
        this.Role = role;
    }
    public abstract void Menu();
}

public class SuperAdmin : SystemAdmin
{
    public SuperAdmin(string username, string role) : base(username, role)
    {

    }
    //Insert Menus for Admins here
    public override void Menu()
    {

    }
    public void AddSystemAdministrator(string username, string password)
    {
        string passwordHash = CryptographyHelper.CreateHashValue(password);
        string id = Guid.NewGuid().ToString();

        string sql = $@"
            INSERT INTO User (Id, Username, PasswordHash, Role)
            VALUES ('{id}', '{username}', '{passwordHash}', 'System Administrator')
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("System Administrator toegevoegd!");
    }

    public void UpdateSystemAd(string currentUsername, string newUsername, string newPassword)
    {
        string passwordHash = CryptographyHelper.CreateHashValue(newPassword);

        string sql = $@"
            UPDATE User
            SET Username = '{newUsername}', PasswordHash = '{passwordHash}'
            WHERE Username = '{currentUsername}' AND Role = 'System Administrator'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine("System Administrator bijgewerkt (of niet gevonden).");
    }
    public void DeleteSystemAd(string username)
    {
        string sql = $@"
            DELETE FROM User
            WHERE Username = '{username}' AND Role = 'System Administrator'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine($"System Administrator verwijderd: {username}");
    }

    public void ResetSystemAdPassword(string username)
    {
        string tempPassword = "Temp" + new Random().Next(1000, 9999);
        string passwordHash = CryptographyHelper.CreateHashValue(tempPassword);

        string sql = $@"
            UPDATE User
            SET PasswordHash = '{passwordHash}'
            WHERE Username = '{username}' AND Role = 'System Administrator'
        ";

        DatabaseHelper.ExecuteStatement(sql);
        Console.WriteLine($"Tijdelijk wachtwoord voor {username}: {tempPassword}");
    }
}



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
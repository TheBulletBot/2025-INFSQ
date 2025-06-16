public class User
{
    public string Username{ get; }
    public string Role{ get; }
    public string PasswordHash{ get; }
    public User(string username, string role, string passwordHash)
    {
        this.Username = username;
        this.Role = role;
        this.PasswordHash = passwordHash;
    }
}
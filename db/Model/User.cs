public class User
{
    public string Username { get; }
    public string Role { get; }
    private string PasswordHash { get; }
    public string FirstName{ get; }
    public string LastName{ get; }
    public User(string username, string role, string passwordHash, string firstName, string lastName)
    {
        this.Username = username;
        this.Role = role;
        this.PasswordHash = passwordHash;
        this.FirstName = firstName;
        this.LastName = lastName;
    }
    
    
}
public class DBUser
{
    public string Username { get; }
    public string Role { get; }
    public string PasswordHash { get; }
    public string FirstName{ get; }
    public string LastName{ get; }
    public string RegistrationDate{ get; }
    public DBUser(string username, string role, string passwordHash, string firstName, string lastName, string registrationDate)
    {
        this.Username = username;
        this.Role = role;
        this.PasswordHash = passwordHash;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.RegistrationDate = registrationDate;
    }
    
    
}
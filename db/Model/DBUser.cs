public class DBUser
{
    public int Id { get; }
    public string Username { get; }
    public string PasswordHash { get; }
    public string Role { get; }
    public string FirstName { get; }
    public string LastName{ get; }
    public string RegistrationDate{ get; }
    public DBUser(int Id, string username, string role, string passwordHash, string firstName, string lastName, string registrationDate)
    {
        this.Id = Id;
        this.Username = username;
        this.Role = role;
        this.PasswordHash = passwordHash;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.RegistrationDate = registrationDate;
    }
    
    
}
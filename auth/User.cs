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
}
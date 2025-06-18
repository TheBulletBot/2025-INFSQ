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

public class ServiceEngineer : User
{
    public ServiceEngineer(string username, string role) : base(username, role)
    {

    }
    //Insert Menus for Service Engineers here.
    public override void Menu()
    {
        
    }
    //Insert all functions that Service Engineers can perform here. 



}

public class Admin : ServiceEngineer
{
    public Admin(string username, string role) : base(username, role)
    {

    }
    //Insert Menus for Admins here
    public override void Menu()
    {

    }

    //Insert all functions that Only Admins can perform here
}

public class SuperAdmin : Admin
{
    public SuperAdmin(string username, string role) : base(username, role)
    {

    }
    //Insert Menus for Admins here
    public override void Menu()
    {

    }
}

enum Role : int
{
    SERVICEENGINEER = 3,
    ADMIN = 2,
    SUPERADMIN = 1
}
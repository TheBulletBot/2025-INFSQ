public static class Login
{

    public static void LoginScreen()
    {
        System.Console.WriteLine("Welcome to ScooterVille's Scooter Rental Management System\n \nPlease Log in.\n\n\n");
        System.Console.WriteLine("Username: ");
        var username = Console.ReadLine();
        System.Console.WriteLine("\nPassword: ");
        var password = HashCode.Combine(Console.ReadLine());

        System.Console.WriteLine($"DEBUG: Username = {username}; PasswordHash = {password}");

        //fetch User with username = 
    }
}

using System.Text.RegularExpressions;

public static class Validation
{
    public static readonly string UsernameRe = @"[a-zA-Z\.'0-9_]{8,10}";
    public static readonly string PasswordRe = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~!@#$%&_\-+=`|\\(){}\[\]:;'<>,.?/])[a-zA-Z\d~!@#$%&_\-+=`|\\(){}\[\]:;'<>,.?/]{12,30}$";
    public static readonly string ZipCodeRe = @"^\d{4}[A-Z]{2}$";
    public static readonly string PhoneRe = @"^\d{8}$";
    public static readonly string LicenseRe = @"^[A-Z]{1,2}\d{7}$";
    public static readonly string IdRe = @"^\d+$";
    public static readonly string BrandRe = @"^[a-zA-Z0-9\s\-]{2,20}$";
    public static readonly string ModelRe = @"^[a-zA-Z0-9\s\-]{1,20}$";
    public static readonly string LocationRe = @"^[a-zA-Z0-9\s,.'\-]{2,30}$";
    public static readonly string NameRe = @"^[a-zA-ZÀ-ÿ\-'\s]{2,30}$";
    public static readonly string EmailRe = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    public static string ValidatedInput(string pattern, string message, string errorMessage)
    {
        System.Console.WriteLine(message);
        string userResponse = Console.ReadLine()!;
        while (true)
        {
            if (Regex.IsMatch(userResponse, pattern))
            {
                return userResponse;
            }
            System.Console.WriteLine(errorMessage);
            System.Console.WriteLine(message);
            userResponse = Console.ReadLine();
        }
    }
}
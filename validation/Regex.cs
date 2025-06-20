
using System.Text.RegularExpressions;

public static class Validation
{
    public static readonly string UsernameRe = @"[a-zA-Z\.'0-9_]{8,10}";
    public static readonly string PasswordRe = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[~!@#$%&_\-+=`|\\(){}\[\]:;'<>,.?/])[a-zA-Z\d~!@#$%&_\-+=`|\\(){}\[\]:;'<>,.?/]{12,30}$";
}
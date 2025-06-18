
using System.Text.RegularExpressions;

public static class Validation
{
    static Regex UsernameRe = new("""[a-zA-Z\.'0-9_]{8,10}""");
    static Regex PasswordRe = new("""^((?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9]).{11,30})\S$""");
}
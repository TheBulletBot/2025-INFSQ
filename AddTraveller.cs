using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class TravelerFunc
{
    private readonly string _connectionString =
        @"Data Source=C:\Users\rensg\OneDrive\Documenten\GitHub\2025-INFSQ-1\ScooterBackend\db\db\INFSQScooterBackend.db";
    private readonly byte[] _aesKey = Encoding.UTF8.GetBytes("1234567890ABCDEF");   // exact 16 bytes
    private readonly byte[] _aesIV  = Encoding.UTF8.GetBytes("FEDCBA0987654321");   // exact 16 bytes

    public void AddTraveler(string username, string password,
        string firstName, string lastName, DateTime birthday, string gender,
        string street, string houseNumber, string zipCode, string city,
        string email, string phone, string license)
    {
        // Validatie
        if (!Regex.IsMatch(zipCode, @"^\d{4}[A-Z]{2}$"))
            throw new ArgumentException("Ongeldige postcode");

        if (!Regex.IsMatch(phone, @"^\d{8}$"))
            throw new ArgumentException("Ongeldig telefoonnummer");

        if (!Regex.IsMatch(license, @"^[A-Z]{1,2}\d{7}$"))
            throw new ArgumentException("Ongeldig rijbewijsnummer");

        if (password.Length < 4)
            throw new ArgumentException("Wachtwoord moet minimaal 4 tekens zijn.");

        int passwordHash = IntHashPassword(password);
        string registrationDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        string encryptedPhone = Encrypt(phone);
        string encryptedStreet = Encrypt(street);
        string encryptedCity = Encrypt(city);

        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string sql = @"INSERT INTO Traveller 
                (Username, PasswordHash, FirstName, LastName, Birthday, Gender,
                 Street, HouseNumber, ZipCode, City, Mail, Phone, LicenseNumber, RegistrationDate)
                VALUES
                (@Username, @PasswordHash, @FirstName, @LastName, @Birthday, @Gender,
                 @Street, @HouseNumber, @ZipCode, @City, @Mail, @Phone, @LicenseNumber, @RegistrationDate)";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Birthday", birthday.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Street", encryptedStreet);
                cmd.Parameters.AddWithValue("@HouseNumber", houseNumber);
                cmd.Parameters.AddWithValue("@ZipCode", zipCode);
                cmd.Parameters.AddWithValue("@City", encryptedCity);
                cmd.Parameters.AddWithValue("@Mail", email);
                cmd.Parameters.AddWithValue("@Phone", encryptedPhone);
                cmd.Parameters.AddWithValue("@LicenseNumber", license);
                cmd.Parameters.AddWithValue("@RegistrationDate", registrationDate);

                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine("✅ Traveller succesvol toegevoegd.");
    }

    private string Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _aesKey;
            aes.IV = _aesIV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(encrypted);
        }
    }

    private int IntHashPassword(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToInt32(hashBytes, 0); // Eerste 4 bytes → int
        }
    }

    public string Decrypt(string encryptedText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _aesKey;
            aes.IV = _aesIV;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}

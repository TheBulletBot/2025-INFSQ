using System;
using System.Collections.Generic;

/*
public class TravelerFunc
{
    public void AddTraveler(string firstName, string lastName, DateTime birthday, string gender,
    string street, string houseNumber, string zipCode, string city,
    string email, string phone, string license)
    {
        if (!!Regex.IsMatch(zipCode, @"^\d{4}[A-Z]{2}$"))
        {
            throw new ArgumentException("ongeldige postcode");
        }
        if (!Regex.IsMatch(phone, @"^\d{8}$"))
        {
            throw new ArgumentException("Ongeldig telefoonnummer");
        }

        if (!Regex.IsMatch(license, @"^[A-Z]{1,2}\d{7}$"))
        {
            throw new ArgumentException("Ongeldig rijbewijsnummer");
        }
        string registrationDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        string customerId = Guid.NewGuid().ToString();

        using (var conn = new SQLiteConnection(_connectionString))
        {
            conn.Open();
            string sql = @"INSERT INTO Travellers 
                (CustomerId, FirstName, LastName, Birthday, Gender,
                 Street, HouseNumber, ZipCode, City, Email, Phone, DrivingLicenseNumber, RegistrationDate)
                VALUES
                (@CustomerId, @FirstName, @LastName, @Birthday, @Gender,
                 @Street, @HouseNumber, @ZipCode, @City, @Email, @Phone, @License, @RegistrationDate)";

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerId", t.CustomerId);
                cmd.Parameters.AddWithValue("@FirstName", t.FirstName);
                cmd.Parameters.AddWithValue("@LastName", t.LastName);
                cmd.Parameters.AddWithValue("@Birthday", t.Birthday.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Gender", t.Gender);
                cmd.Parameters.AddWithValue("@Street", encryptedStreet);
                cmd.Parameters.AddWithValue("@HouseNumber", t.HouseNumber);
                cmd.Parameters.AddWithValue("@ZipCode", t.ZipCode);
                cmd.Parameters.AddWithValue("@City", encryptedCity);
                cmd.Parameters.AddWithValue("@Email", t.Email); // ook te versleutelen indien gewenst
                cmd.Parameters.AddWithValue("@Phone", encryptedPhone);
                cmd.Parameters.AddWithValue("@License", t.DrivingLicenseNumber);
                cmd.Parameters.AddWithValue("@RegistrationDate", t.RegistrationDate.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine("Traveller succesvol toegevoegd.");
    }

}
*/
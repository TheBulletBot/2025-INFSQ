public class Traveler
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Birthday { get; }
    public string Gender { get; }
    public string StreetName { get; }
    public string HouseNumber { get; }
    public string ZipCode { get; }
    public string City { get; }
    public string Email { get; }
    public string Phone { get; }
    public string LicenseNumber { get; }

    public Traveler(
        int id,
        string FirstName,
        string LastName,
        string Birthday,
        string Gender,
        string StreetName,
        string HouseNumber,
        string ZipCode,
        string City,
        string Email,
        string Phone,
        string LicenseNumber)
    {
        this.Id = id;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Birthday = Birthday;
        this.Gender = Gender;
        this.StreetName = StreetName;
        this.HouseNumber = HouseNumber;
        this.ZipCode = ZipCode;
        this.City = City;
        this.Email = Email;
        this.Phone = Phone;
        this.LicenseNumber = LicenseNumber;
    }
}
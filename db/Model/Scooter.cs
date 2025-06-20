public class Scooter
{
    public string SerialNumber { get; }
    public string Brand { get; }
    public string Model { get; }
    public int TopSpeed { get; }
    public int BatteryCapacity { get; }
    public int StateOfCharge { get; }
    public string TargetRange{ get; }
    public string Location { get; }
    public int OutOfService { get; }
    public float Mileage { get; }
    public string LastMaintenance { get; }

    public Scooter(
        string SerialNumber,
        string Brand,
        string Model,
        int TopSpeed,
        int BatteryCapacity,
        int StateOfCharge,
        string TargetRange,

        string Location,
        int OutOfService,
        float Mileage,
        string LastMaintenance)
    {
        this.SerialNumber = SerialNumber;
        this.Brand = Brand;
        this.Model = Model;
        this.TopSpeed = TopSpeed;
        this.BatteryCapacity = BatteryCapacity;
        this.StateOfCharge = StateOfCharge;
        this.TargetRange = TargetRange;
        this.Location = Location;
        this.OutOfService = OutOfService;
        this.Mileage = Mileage;
        this.LastMaintenance = LastMaintenance;
    }


}
public class Scooter
{
    public string SerialNumber { get; }
    public string Brand { get; }
    public string Model { get; }
    public int TopSpeed { get; }
    public int BatteryCapacity { get; }
    public int StateOfCharge { get; }
    public int TargetMax { get; }
    public int TargetMin { get; }
    public string Location { get; }
    public bool OutOfService { get; }
    public float Mileage { get; }
    public DateOnly LastMaintenance { get; }

    public Scooter(
        string SerialNumber,
        string Brand,
        string Model,
        int TopSpeed,
        int BatteryCapacity,
        int StateOfCharge,
        int TargetMax,
        int TargetMin,
        string Location,
        bool OutOfService,
        float Mileage,
        DateOnly LastMaintenance)
    {
        this.SerialNumber = SerialNumber;
        this.Brand = Brand;
        this.Model = Model;
        this.TopSpeed = TopSpeed;
        this.BatteryCapacity = BatteryCapacity;
        this.TargetMax = TargetMax;
        this.TargetMin = TargetMin;
        this.Location = Location;
        this.OutOfService = OutOfService;
        this.Mileage = Mileage;
        this.LastMaintenance = LastMaintenance;
    }


}
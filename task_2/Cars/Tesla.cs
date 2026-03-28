using CarFactory.Interfaces;

namespace CarFactory.Cars;

public class Tesla : ACar, IElectric, IAutomatical
{
    public int BatteryCapacity { get; }
    public string TransmissionType { get; }

    public Tesla() : base("Tesla", 5, true)
    {
        BatteryCapacity = 100;
        TransmissionType = "автоматическая";
    }

    public override string GetDescription()
    {
        return $"{Brand}: {GetEngineType()} с {GetTransmissionType()}, {Seats} местами, {GetAndroidStatus()}";
    }
}

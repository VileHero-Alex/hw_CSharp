using CarFactory.Interfaces;

namespace CarFactory.Cars;

public class Toyota : ACar, IGas, IAutomatical
{
    public string FuelType { get; }
    public string TransmissionType { get; }

    public Toyota() : base("Toyota", 5, false)
    {
        FuelType = "бензин";
        TransmissionType = "автоматическая";
    }

    public override string GetDescription()
    {
        return $"{Brand}: {GetEngineType()} с {GetTransmissionType()}, {Seats} местами, {GetAndroidStatus()}";
    }
}

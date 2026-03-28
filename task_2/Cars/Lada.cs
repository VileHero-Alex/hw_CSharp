using CarFactory.Interfaces;

namespace CarFactory.Cars;

public class Lada : ACar, IGas, IMechanical
{
    public string FuelType { get; }
    public string TransmissionType { get; }

    public Lada() : base("Lada", 5, false)
    {
        FuelType = "бензин";
        TransmissionType = "механическая";
    }

    public override string GetDescription()
    {
        return $"{Brand}: {GetEngineType()} с {GetTransmissionType()}, {Seats} местами, {GetAndroidStatus()}";
    }
}

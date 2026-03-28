using CarFactory.Interfaces;

namespace CarFactory.Cars;

public class BMW : ACar, IDiesel, IAutomatical
{
    public string FuelType { get; }
    public string TransmissionType { get; }

    public BMW() : base("BMW", 5, true)
    {
        FuelType = "дизель";
        TransmissionType = "автоматическая";
    }

    public override string GetDescription()
    {
        return $"{Brand}: {GetEngineType()} с {GetTransmissionType()}, {Seats} местами, {GetAndroidStatus()}";
    }
}

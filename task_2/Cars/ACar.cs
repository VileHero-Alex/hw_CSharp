using CarFactory.Interfaces;

namespace CarFactory.Cars;

public abstract class ACar : ICar
{
    public string Brand { get; protected set; }
    public int Seats { get; protected set; }
    public bool HasAndroid { get; protected set; }

    protected ACar(string brand, int seats, bool hasAndroid)
    {
        Brand = brand;
        Seats = seats;
        HasAndroid = hasAndroid;
    }

    public abstract string GetDescription();

    protected string GetEngineType()
    {
        if (this is IElectric)
            return "электрокар";
        else if (this is IDiesel)
            return "дизельный автомобиль";
        else if (this is IGas)
            return "бензиновый автомобиль";
        else
            return "автомобиль";
    }

    protected string GetTransmissionType()
    {
        if (this is IAutomatical)
            return "автоматической коробкой передач";
        else if (this is IMechanical)
            return "механической коробкой передач";
        else
            return "коробкой передач";
    }

    protected string GetAndroidStatus()
    {
        return HasAndroid ? "Android на борту" : "без Android";
    }
}

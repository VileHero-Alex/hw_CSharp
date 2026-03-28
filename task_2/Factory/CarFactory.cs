using CarFactory.Interfaces;
using CarFactory.Cars;

namespace CarFactory.Factory;

public class CarFactory
{
    public static ICar CreateCar(CarType type)
    {
        switch (type)
        {
            case CarType.Tesla:
                return new Tesla();
            case CarType.BMW:
                return new BMW();
            case CarType.Toyota:
                return new Toyota();
            case CarType.Lada:
                return new Lada();
            default:
                throw new ArgumentException($"Неизвестный тип автомобиля: {type}");
        }
    }
}

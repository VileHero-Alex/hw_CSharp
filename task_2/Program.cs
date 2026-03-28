using CarFactory.Interfaces;
using CarFactory.Factory;

namespace CarFactory;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Write("Введите марку автомобиля или done для остановки ввода: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            if (input.Trim().ToLower() == "done")
            {
                Console.WriteLine("Программа завершена.");
                break;
            }

            try
            {
                CarType carType = Enum.Parse<CarType>(input.Trim(), ignoreCase: true);
                ICar car = Factory.CarFactory.CreateCar(carType);
                Console.WriteLine(car.GetDescription());
                Console.WriteLine();
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"Ошибка: марка '{input}' не найдена. Доступные марки: Tesla, BMW, Toyota, Lada");
                Console.WriteLine();
            }
        }
    }
}

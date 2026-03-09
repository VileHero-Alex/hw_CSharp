using System;

namespace Calculator {
    class Program {
        static void Main(string[] args) {
            while (true) {
                Console.WriteLine("=== Калькулятор ===");
                Console.WriteLine("Введите 'q' для выхода");

                Console.Write("Введите первое число: ");
                string input1 = Console.ReadLine();

                if (input1 == "q") {
                    Console.WriteLine("Выход из программы...");
                    break;
                }

                double num1 = Convert.ToDouble(input1);

                Console.Write("Введите второе число: ");
                string input2 = Console.ReadLine();

                if (input2 == "q") {
                    Console.WriteLine("Выход из программы...");
                    break;
                }

                double num2 = Convert.ToDouble(input2);

                Console.Write("Введите операцию (+, -, *, /): ");
                string operation = Console.ReadLine();

                if (operation == "q") {
                    Console.WriteLine("Выход из программы...");
                    break;
                }

                double result = 0;

                if (operation == "+") {
                    result = num1 + num2;
                    Console.WriteLine("Результат: " + num1 + " + " + num2 + " = " + result);
                } else if (operation == "-") {
                    result = num1 - num2;
                    Console.WriteLine("Результат: " + num1 + " - " + num2 + " = " + result);
                } else if (operation == "*") {
                    result = num1 * num2;
                    Console.WriteLine("Результат: " + num1 + " * " + num2 + " = " + result);
                } else if (operation == "/") {
                    if (num2 == 0) {
                        Console.WriteLine("Ошибка: деление на ноль!");
                    } else {
                        result = num1 / num2;
                        Console.WriteLine("Результат: " + num1 + " / " + num2 + " = " + result);
                    }
                } else {
                    Console.WriteLine("Ошибка: неверная операция!");
                }

                Console.WriteLine();
            }
        }
    }
}

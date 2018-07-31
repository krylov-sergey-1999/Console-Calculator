using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Calculator
{
    class Program
    {
        // Нет реализации проверки на ошибки ввода
        static void Main(string[] args)
        {
            while (true) //Бесконечный цикл
            {
                Console.Write("Введите выражение: "); //Предлагаем ввести выражение
                Console.WriteLine(RPN.Calculate(Console.ReadLine())); //Считываем, и выводим результат
            }
        }
    }
}

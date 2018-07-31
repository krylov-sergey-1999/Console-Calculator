﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Calculator
{
    /// <summary>
    ///  Алгоритм Обратной польской нотации (ОПН) — форма записи математических выражений, 
    ///  в которой операнды расположены перед знаками операций. 
    ///  Также именуется как обратная польская запись, обратная бесскобочная запись (ОБЗ).
    ///  Reverse Polish Notation — тут будет реализована работа алгоритма
    /// </summary>
    class RPN
    {
        /// <summary>
        /// Метод Calculate принимает выражение в виде строки и возвращает результат, в своей работе использует другие методы класса
        /// </summary>
        /// <param name="input">Выражение</param>
        /// <returns>Ответ</returns>
        static public double Calculate(string input)
        {
            string output = GetExpression(input); //    Преобразовываем выражение в постфиксную запись
            double result = Counting(output); //    Решаем полученное выражение
            return result; //   Возвращаем результат
        }

        /// <summary>
        /// Метод, преобразующий входную строку с выражением в постфиксную запись
        /// </summary>
        /// <param name="input">Выражение</param>
        /// <returns>Строка в виде постфиксной записи</returns>
        static private string GetExpression(string input)
        {
            string output = string.Empty; //    Строка для хранения выражения
            Stack<char> operStack = new Stack<char>(); //   Стек для хранения операторов

            for (int i = 0; i < input.Length; i++) //   Для каждого символа в входной строке
            {
                //Разделители пропускаем
                if (IsDelimeter(input[i]))
                    continue; //    Переходим к следующему символу

                //  Если символ - цифра, то считываем все число
                if (Char.IsDigit(input[i])) //Если цифра
                {
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }

                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                }

                //Если символ - оператор
                if (IsOperator(input[i])) //Если оператор
                {
                    if (input[i] == '(') //Если символ - открывающая скобка
                        operStack.Push(input[i]); //Записываем её в стек
                    else if (input[i] == ')') //Если символ - закрывающая скобка
                    {
                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';
                            s = operStack.Pop();
                        }
                    }
                    else //Если любой другой оператор
                    {
                        if (operStack.Count > 0) //Если в стеке есть элементы
                            if (GetPriority(input[i]) <= GetPriority(operStack.Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                        operStack.Push(char.Parse(input[i].ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека

                    }
                }
            }

            //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output; //Возвращаем выражение в постфиксной записи
        }
  
        /// <summary>
        /// Метод, вычисляющий значение выражения, уже преобразованного в постфиксную запись
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Результат</returns>
        static private double Counting(string input)
        {
            double result = 0; //Результат
            Stack<double> temp = new Stack<double>(); // Стек для решения

            for (int i = 0; i < input.Length; i++) //Для каждого символа в строке
            {
                //Если символ - цифра, то читаем все число и записываем на вершину стека
                if (Char.IsDigit(input[i]))
                {
                    string a = string.Empty;

                    while (!IsDelimeter(input[i]) && !IsOperator(input[i])) //Пока не разделитель
                    {
                        a += input[i]; //Добавляем
                        i++;
                        if (i == input.Length) break;
                    }
                    temp.Push(double.Parse(a)); //Записываем в стек
                    i--;
                }
                else if (IsOperator(input[i])) //Если символ - оператор
                {
                    //Берем два последних значения из стека
                    double a = temp.Pop();
                    double b = temp.Pop();

                    switch (input[i]) //И производим над ними действие, согласно оператору
                    {
                        case '+': result = b + a; break;
                        case '-': result = b - a; break;
                        case '*': result = b * a; break;
                        case '/': result = b / a; break;
                        case '^': result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                    }
                    temp.Push(result); //Результат вычисления записываем обратно в стек
                }
            }
            return temp.Peek(); //Забираем результат всех вычислений из стека и возвращаем его
        }

        /// <summary>
        /// Метод возвращает true, если проверяемый символ - разделитель ("пробел" или "равно")
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        ///   <c>true</c> if the specified c is delimeter; otherwise, <c>false</c>.
        /// </returns>
        static private bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))
                return true;
            return false;
        }

        /// <summary>
        /// Метод возвращает true, если проверяемый символ - оператор
        /// </summary>
        /// <param name="с">The с.</param>
        /// <returns>
        ///   <c>true</c> if the specified с is operator; otherwise, <c>false</c>.
        /// </returns>
        static private bool IsOperator(char с)
        {
            if (("+-/*^()".IndexOf(с) != -1))
                return true;
            return false;
        }

        /// <summary>
        /// Метод возвращает приоритет оператора
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        static private byte GetPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                case '^': return 5;
                default: return 6;
            }
        }
    }
}

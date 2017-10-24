using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace YContest
{
    /*
      
        ИГРА С ЧИСЛАМИ
     
        Дана последовательность n положительных чисел a1,a2,…,an. 
        Пока среди них есть различные, выполняется следующая операция: выбирается некоторое максимальное число и из него вычитается минимальное число.
        Через сколько операций числа станут одинаковыми?

        ФОРМАТ ВВОДА
        В первой строке входных данных задано число n (1≤n≤1000). В следующей строке заданы n чисел ai (1≤ai≤10^9).

        ФОРМАТ ВЫВОДА
        Количество операций, после которых все числа станут одинаковыми.

        ------------------------------
        ПРИМЕР 1
        ------------------------------
        Ввод	
        2
        1 1
        Вывод
        0
        ------------------------------
        ПРИМЕР 2
        ------------------------------
        Ввод	
        3
        9 6 3
        Вывод
        3
        ------------------------------
        ПРИМЕР 3
        ------------------------------
        Ввод	
        6
        1000000000 1000000000 1000000000 1000000000 1000000000 1
        Вывод
        4999999995
        ------------------------------
    */

    class TaskF_Sofi
    {

        public static void Solve(StreamReader rdr, StreamWriter wr)
        {
            var count = int.Parse(rdr.ReadLine());
            var numbers = rdr.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();

            if (count != numbers.Length)
                throw new Exception();

            var sortedSet = new SortedList<int, int>();
            foreach (var uniqueNumber in numbers.GroupBy(x => x))
            {
                sortedSet.Add(uniqueNumber.Key, uniqueNumber.Count());
            }

            wr.Write(Calc(sortedSet));
        }
        
        private static long Calc(SortedList<int, int> numbers)
        {
            long iterations = 0;

            while (numbers.Count() > 1)
            {
                var min = numbers.First();
                var max = numbers.Last();

                var currentMax = max;

                var quotient  = (currentMax.Key - min.Key) / min.Key;
                while (quotient > 0)
                {
                    iterations += (long)quotient * currentMax.Value;
                    numbers.Remove(currentMax.Key);

                    var newKey = currentMax.Key - quotient * min.Key;
                    AddKey(numbers, newKey);

                    numbers[newKey] += currentMax.Value;
                    currentMax = numbers.Last();
                    quotient = (currentMax.Key - 2 * min.Key) / min.Key;
                }

                if (numbers.Count() == 1)
                    break;

                max = numbers.Last();

                if (max.Value == 1)
                    numbers.Remove(max.Key);
                else
                    numbers[max.Key] = max.Value - 1; ;

                AddKey(numbers, max.Key - min.Key);
                numbers[max.Key - min.Key]++;

                iterations++;
            }

            return iterations;
        }

        private static void AddKey(SortedList<int, int> numbers, int newKey)
        {
            if (!numbers.ContainsKey(newKey))
                numbers.Add(newKey, 0);
        }
    }
}

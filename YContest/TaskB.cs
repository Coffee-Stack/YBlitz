using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YContest
{
    class TaskB
    {
        // Обозначим через S(n) сумму цифр натурального числа n.
        // Будем говорить, что натуральное число n сложное, если не существует такого натурального числа k, что n = 3*k/s(k)^2
        // Найдите наименьшее сложное число.

        private static HashSet<int> nums = new HashSet<int>(Enumerable.Range(1, 10000000));
        private static ConcurrentBag<int> cnums = new ConcurrentBag<int>();

        public static void Solve(StreamReader rdr, StreamWriter wr)
        {
            Parallel.ForEach(nums, num =>
            {
                int s = S(num);
                var res = 3 * num / (double)(s * s);
                if (res == (int) res)
                    cnums.Add((int) res);
            });

            foreach (var cnum in cnums)
                nums.Remove(cnum);

            wr.Write($"Minimal:{nums.Min()}");
        }

        private static int S(int number)
        {
            int res = 0;
            while (number != 0)
            {
                res += number % 10;
                number /= 10;
            }
            return res;
        }
    }
}
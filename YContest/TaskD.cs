using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YContest
{
    // Программист Петя очень любит складывать все имеющиеся у него деньги в кошельки и фиксировать, 
    // сколько денег лежит в каждом кошельке. Для этого он сохраняет в файле набор целых положительных 
    // чисел — количество денег, которое лежит в каждом из его кошельков (Петя не любит, когда хотя бы 
    // один из его кошельков пустует). Петя хранит все деньги в монетах, номинал каждой монеты — 1 условная 
    // единица.
    // 
    // Однажды у Пети сломался блок магнитных головок и ему пришлось восстанавливать данные с жесткого диска. 
    // Он хочет проверить, корректно ли восстановились данные, и просит вас убедиться, что можно ту сумму денег, 
    // которая у него была, разложить во все его кошельки, чтобы получились те же числа, что и в восстановленном 
    // файле.

    // Формат ввода
    // В первой строке выходных данных содержится натуральное число n(1≤n≤100) — количество кошельков у Пети.
    // Во второй строке через пробел записаны данные из восстановленного файла: n натуральных чисел ai, 
    // каждое из которых означает, сколько денег лежит в i-м кошельке у Пети (1≤ai≤100).
    // В третьей строке записано натуральное число m(1≤m≤10^4) — общая сумма денег, которая была 
    // у Пети до того, как он разложил её по кошелькам.

    // Пример 1
    // Ввод 	Вывод
    // 2        Yes
    // 2 3
    // 5

    // Пример 2
    // Ввод 	Вывод
    // 2		No
    // 2 3
    // 4

    // Пример 3
    // Ввод 	Вывод
    // 2		Yes
    // 2 3
    // 3

    // Примечания
    // В первом примере у Пети есть два кошелька, в первом лежат две монеты, во втором — три.
    // Конфигурации, приведенной во втором примере, не может существовать, поэтому файл восстановлен некорректно.
    // В третьем примере предложенная конфигурация возможна: во втором кошельке лежит одна монета и первый кошелёк, 
    // внутри которого лежат две монеты. 

    public static class TaskD
    {
        public static void Solve(StreamReader rdr, StreamWriter wr)
        {
            rdr.ReadLine();
            var wallets = rdr.ReadLine().Split(' ').Select(int.Parse).ToList();
            var walletsFilling = new List<int>(wallets.Count);

            wallets.Sort();

            int count = int.Parse(rdr.ReadLine());

            var max = wallets.Sum();
            var min = wallets.Last();

            if (count > max
                || count < min
                || count < min + wallets.First())
            {
                wr.Write("No");
                return;
            }

            if (count == min || count == min + wallets.First())
            {
                wr.Write("Yes");
                return;
            }

            wr.Write(CheckSubSum(count - min, wallets, walletsFilling, wallets.Count - 2) ? "Yes" : "No");
        }

        private static bool CheckSubSum(int num, List<int> wallets, List<int> walletsFilling, int pointer)
        {
            if (wallets[pointer] == num)
                return true;

            if (pointer > 0)
                return (wallets[pointer] != wallets[pointer - 1] &&
                        CheckSubSum(num, wallets, walletsFilling, pointer - 1)) ||
                       CheckSubSum(num - wallets[pointer], wallets, walletsFilling, pointer - 1);
            return false;
        }
    }
}
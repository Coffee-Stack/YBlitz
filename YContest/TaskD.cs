using System;
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
            var wallets = rdr.ReadLine().Split(' ').Select(int.Parse).Select(el => new Wallet {Size = el}).ToList();
            wallets.Sort(Comparer<Wallet>.Create((w1, w2) => w1.Size.CompareTo(w2.Size)));

            int count = int.Parse(rdr.ReadLine());

            var max = wallets.Select(el => el.Size).Sum();

            if (count > max)
            {
                wr.Write("No");
                return;
            }
            
            wr.Write(CheckSubSum(count, wallets, max, 0) ? "Yes" : "No");
        }

        private static bool CheckSubSum(int num, List<Wallet> wallets, int max, int pointer)
        {
            if (pointer == wallets.Count || max < num)
                return false;

            if (num == max)
                return true;

            bool withNesting = TryNest(wallets, pointer) &&
                               CheckSubSum(num, wallets, max - wallets[pointer].Size, pointer + 1);

            if (withNesting)
                return true;

            if (wallets[pointer].In != null)
                TakeOut(wallets[pointer]);

            bool withoutNesting = CheckSubSum(num, wallets, max, pointer + 1);
            return withoutNesting;
        }

        private static void TakeOut(Wallet wallet)
        {
            var inw = wallet.In;
            inw.Filling -= wallet.Size;
            wallet.In = null;
        }

        private static bool TryNest(List<Wallet> wallets, int pointer)
        {
            var current = wallets[pointer];
            for (int i = pointer + 1; i < wallets.Count; i++)
            {
                if (wallets[i].Size == current.Size)
                    continue;

                if (wallets[i].Filling + current.Size <= wallets[i].Size - 1)
                {
                    wallets[i].Filling += current.Size;
                    current.In = wallets[i];
                    return true;
                }
            }

            return false;
        }
    }

    class Wallet
    {
        public Wallet In { get; set; }
        public int Size { get; set; }
        public int Filling { get; set; }

        public override string ToString()
        {
            return $"[Size:{Size},Filling:{Filling},Nested:{In != null}]";
        }
    }
}
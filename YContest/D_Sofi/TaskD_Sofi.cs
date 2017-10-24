using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.IO;

namespace YContest
{
    /*
    Программист Игорь очень любит играть в компьютерные игры. Больше всего Игорю нравятся стратегии, 
    особенно те моменты, когда он отправляет группы своих юнитов атаковать вражеские базы. 
    Игорь довольно давно играет в стратегии, поэтому у него есть чётко отработанный план действий при атаке: 
    он разбивает все свои юниты на группы и отправляет их в атаку поочередно. 
    При этом Игорь считает, что общий урон, который будет нанесен в атаке, равен произведению размеров групп. 
    Он пытается разбить свои юниты на группы так, чтобы максимизировать общий урон.
    В последнее время Игорь стал часто проигрывать. Он уверен, что проблема в том, что в одной из групп 
    после разбиения получается несчастливое количество юнитов. Он пытается переделать свой алгоритм 
    разбиения и попросил вас посчитать, какой максимальный общий урон смогут нанести его группы, 
    если среди них не будет ни одной, содержащей несчастливое количество юнитов.

    ФОРМАТ ВВОДА

    В единственной строке входных данных через пробел записаны натуральные числа n и a 
    (1≤n, a≤10^6, n≠a) — количество юнитов, которые есть у Игоря, и число юнитов в группе, которое Игорь считает несчастливым.

    ФОРМАТ ВЫВОДА

    Выведите максимально возможный общий урон по модулю 10^9+7.

    ПРИМЕР 1
    Ввод
    8 2
    Вывод
    16

    ПРИМЕР 2
    Ввод
    9 3
    Вывод
    20

    ПРИМЕЧАНИЯ
    В первом примере для максимизации общего урона следует разбить юниты на две группы по 4 юнита в каждом, 
    во втором примерe — на две группы: в первой 4 юнита, во второй — 5.
    */

    class TaskD_Sofi
    {
        private const int _modBase = 1000000007;

        public static void Solve(StreamReader rdr, StreamWriter wr)
        {
            var input = rdr.ReadLine().Split(new[] { ' ' }).Select(x=> int.Parse(x)).ToArray();
            var maxDamage = CalcMaxDamage(input[0], input[1]);
            wr.Write(maxDamage);
        }
        
        private static long Mod(BigInteger n, int modBase)
        {
            return (long)BigInteger.Remainder(n + modBase, modBase);
        }

        private static long CalcMaxDamage(int unitCount, int unlucky)
        {
            BigInteger damage = 0;

            if (unitCount == 1)
            {
                damage = 1;
            }
            else
            {
                if (unlucky == 3)
                {
                    var d = 2;
                    var groupCount = unitCount / d;
                    var tail = unitCount % d;

                    if (tail == 0)
                    {
                        damage = BigInteger.Pow(2, groupCount);
                    }
                    else
                    {
                        if (groupCount > 1)
                            damage = BigInteger.Pow(d, groupCount - 2) * 5;
                        else
                            damage = 1;
                    }
                }
                else
                {
                    var d = 3;
                    var groupCount = unitCount / d;
                    var tail = unitCount % d;

                    switch (tail)
                    {
                        case 1:
                            damage = BigInteger.Pow(new BigInteger(d), groupCount - 1) * 4;
                            break;
                        case 2:
                            if (unlucky != 2)
                                damage = BigInteger.Pow(new BigInteger(d), groupCount) * 2;
                            else
                            {
                                if (groupCount == 1)
                                    damage = BigInteger.Pow(new BigInteger(d), groupCount - 1) * 5;
                                else
                                    damage = BigInteger.Pow(new BigInteger(d), groupCount - 2) * 16;
                            }
                            break;
                        default:
                            damage = BigInteger.Pow(new BigInteger(d), groupCount);
                            break;
                    }
                }
            }

            return Mod(damage, _modBase);
        }
    }
}

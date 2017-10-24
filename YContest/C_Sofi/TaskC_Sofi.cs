using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace YContest
{
    // ТУРНИРНАЯ ТАБЛИЦА

    //  Как-то раз одна компания программистов решила провести соревнования по спортивному программированию, чтобы выяснить, 
    //  кто из них лучше всего решает задачи.Соревнование проходило в два этапа: сначала все решали одинаковый набор задач, 
    //  после чего наступал этап «взлома»: каждый из участников мог посмотреть решение любого другого участника и найти в нём ошибку.
    //  К сожалению, у компьютера, на котором проводилось соревнование, сломался жесткий диск.
    //  Программисты сумели восстановить только часть данных и теперь у них есть статистика каждого участника после этапа решения 
    //  и информация обо всех попытках взлома.Они попросили вас помочь им по имеющимся данным восстановить турнирную таблицу.

    // ФОРМАТ ВВОДА

    //  Входные данные представляют собой два набора строк: статистика каждого из участников после этапа решения и попытки взлома чужих решений.
    //  Каждая строка первой части состоит их нескольких полей: вначале идет слово Result, затем имя участника,
    //  после чего набор пар «идентификатор задачи и количество заработанных очков». Каждое имя состоит только из латинских букв,
    //  начинается с заглавной буквы, все остальные буквы строчные. Идентификатор задачи — заглавная латинская буква.
    //  Количество заработанных очков за каждую задачу — целое неотрицательное число, не превосходящее 10000. 

    //  Если информации по какой-либо задаче нет, считается, что участник получил за неё ноль очков.
    //  Каждая строка второй части состоит из четырех полей. Первое поле — имя участника, который пытается совершить взлом,
    //  второе поле — имя участника, чьё решение он пытается взломать, третье поле — идентификатор задачи, четвёртое — результат взлома. 

    //  Ограничения на первые три поля совпадают с первой частью ввода, четвёртое поле может принимать значения OK, если взлом успешный, или FAIL,
    //  если взлом не удался. Если взлом был успешным, участник, взламывавший решение, получает 50 дополнительных очков, а участник,
    //  чьё решение взломали, теряет все очки, набранные за взломанную задачу на этапе решения.
    //  Если взлом неуспешен, участник, взламывавший решение, теряет 25 очков.

    //  Гарантируется, что выходные данные удовлетворяют следующим условиям:
    //    - Длина каждого имени не превосходит 30 символов.
    //    - Каждый участник будет пытаться взломать задачу другого участника не больше одного раза.
    //    - Общее число участников не превосходит 100, а общее число взломов не больше 1000.
    //    - Уже взломанную задачу участника не будут взламывать другие участники.

    // ФОРМАТ ВЫВОДА

    //  Вам нужно построить турнирную таблицу с результатами.
    //  Каждая строка таблицы — представление результатов каждого из участников.
    //  В первом столбце содержится порядковый номер участника, во втором  — его имя.
    //  Далее следует N столбцов, в каждом из которых содержится информация о количестве очков, набранных участником 
    //  за каждую из N предложенных задач. Если задача была взломана другим участником, то в колонке должно быть число 0, 
    //  иначе — количество очков, набранных участником на этапе решения.Первый столбец содержит количество очков по задаче A,
    //  второй – по задаче B, и т. д. В последнем столбце должно быть выписано общее количество очков, набранных участником. 
    //  Оно рассчитывается как сумма всех набранных им очков за решение задач и количество очков, набранное им на этапе взлома.
    //  Если участник набрал отрицательное количество очков, его результат должен быть равен нулю.
    //  В итоговой таблице участники должны быть отсортированы по убыванию набранных очков.
    //  Если двое или более участников набрали одинаковое количество очков, то они должны быть упорядочены в лексикографическом порядке их имен.
    //  Все столбцы должны иметь минимально возможную ширину, чтобы вместить данные в каждой строке. 
    //  Для столбцов, содержащих порядковые номера и очки, все данные выравниваются по правому краю, 
    //  все имена выравниваются по левому краю, после каждого имени в таблице должен гарантированно присутствовать пробел.
    //  Оформляя таблицу, ориентируйтесь на примеры.

    /*
    
        *** Пример 1 ***

        Ввод 
            Result Vasya A 500 B 2500 D 450
            Result Petya A 100 B 200 C 5000 D 500
            Result Kolya A 10 E 10
            Kolya Vasya A FAIL
            Kolya Vasya B FAIL
            Petya Vasya A OK
            Vasya Petya C FAIL

        Вывод
            +-+------+---+----+----+---+--+----+
            |1|Petya |100| 200|5000|500| 0|5850|
            +-+------+---+----+----+---+--+----+
            |2|Vasya |  0|2500|   0|450| 0|2925|
            +-+------+---+----+----+---+--+----+
            |3|Kolya | 10|   0|   0|  0|10|   0|
            +-+------+---+----+----+---+--+----+

        *** Пример 2 ***

        Ввод	
            Result Vasya A 500 B 2500
            Result Petya A 2500 B 500
            Result Kolya A 3000

        Вывод
            +-+------+----+----+----+
            |1|Kolya |3000|   0|3000|
            +-+------+----+----+----+
            |2|Petya |2500| 500|3000|
            +-+------+----+----+----+
            |3|Vasya | 500|2500|3000|
            +-+------+----+----+----+
    */

    public static class TaskC_Sofi
    {
        public static void Solve(StreamReader input, StreamWriter output)
        {
            var scores = new Dictionary<string, Dictionary<string, int>>();
            ReadScores(scores, input);
            WriteResults(scores, output);
        }

        private static void ReadScores(Dictionary<string, Dictionary<string, int>> scores, StreamReader reader)
        {
            bool firstPart = true;
            string line;
            while (!reader.EndOfStream && (line = reader.ReadLine()) != "")
            {
                var parts = line?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts == null)
                    continue;

                firstPart = firstPart && parts[0] == "Result";

                if (firstPart)
                {
                    if (parts.Length >= 2)
                    {
                        var playerName = parts[1];
                        playerName = playerName.Substring(0, Math.Min(30, playerName.Length));
                        if (!scores.ContainsKey(playerName))
                            scores.Add(playerName, new Dictionary<string, int>());

                        var currentScore = scores[playerName];

                        for (int i = 2; i < parts.Length; i = i + 2)
                        {
                            currentScore.Add(parts[i], int.Parse(parts[i + 1]));
                        }
                    }
                }
                else
                {
                    var brokenTasks = new List<BrokenTask>();

                    if (parts.Length < 4 || (parts[3] != "OK" && parts[3] != "FAIL"))
                        continue;

                    var brokenTask = new BrokenTask()
                    {
                        Player = parts[0],
                        Victim = parts[1],
                        Task = parts[2],
                        Result = parts[3] == "OK"
                    };

                    if (brokenTasks.Any(x => string.Equals(x.Player, brokenTask.Player)
                                             && string.Equals(x.Victim, brokenTask.Victim)
                                             && string.Equals(x.Task, brokenTask.Task)))
                        continue;

                    if (brokenTasks.Any(x => string.Equals(x.Victim, brokenTask.Victim)
                                             && string.Equals(x.Task, brokenTask.Task)
                                             && x.Result))
                        continue;

                    brokenTasks.Add(brokenTask);

                    if (!scores.ContainsKey(brokenTask.Player))
                        scores.Add(brokenTask.Player, new Dictionary<string, int>());

                    if (!scores.ContainsKey(brokenTask.Victim))
                        scores.Add(brokenTask.Victim, new Dictionary<string, int>());

                    var player = scores[brokenTask.Player];
                    var victim = scores[brokenTask.Victim];

                    if (!player.ContainsKey("###ADD"))
                        player.Add("###ADD", 0);

                    if (brokenTask.Result)
                    {
                        player["###ADD"] += 50;
                        if (!victim.ContainsKey(brokenTask.Task))
                            victim.Add(brokenTask.Task, 0);
                        victim[brokenTask.Task] = 0;
                    }
                    else
                    {
                        player["###ADD"] -= 25;
                    }
                }

            }
        }
        
        private static void WriteResults(Dictionary<string, Dictionary<string, int>> scores, StreamWriter writer)
        {
            if (scores == null || !scores.Any())
                return;

            foreach (var player in scores)
            {
                int sum = player.Value.Values.Any() ? player.Value.Values.Sum(x => x) : 0;
                if (sum < 0)
                    sum = 0;
                player.Value.Add("###SUM", sum);
            }

            var cols = new List<LeadTableColumn>();

            // количество участников
            var countOfParticipants = scores.Count;
            cols.Add(new LeadTableColumn(1, "", countOfParticipants < 10 ? 1 : (countOfParticipants == 100 ? 3 : 2), false));

            // имена участников
            var maxNameLenght = scores.Keys.Count == 0 ? 0 : scores.Keys.Max(x => string.IsNullOrWhiteSpace(x) ? 0 : x.Length);
            cols.Add(new LeadTableColumn(2, "", maxNameLenght + 1, false));

            // задачи
            var lastLetter = scores.Values.SelectMany(x => x.Keys).Where(x => !string.Equals(x, "###ADD") && !string.Equals(x, "###SUM")).Distinct().OrderBy(x => x).Last();
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var allTasks = alpha.Select(x => x.ToString());
            int i = 1;
            foreach (var t in allTasks)
            {
                var maxLenght = scores.Values.Select(x => x.ContainsKey(t) ? x[t] : 0).Select(x => x.ToString()).Max(x => x.Length);
                cols.Add(new LeadTableColumn(2 + i, t, maxLenght));
                i++;

                if (string.Equals(lastLetter, t))
                    break;
            }

            // общие баллы
            var tmpListCommonScores = scores.Values.Select(x => x["###SUM"]).Select(x => x.ToString());
            var commonScoreMaxLenght = tmpListCommonScores.Any() ? tmpListCommonScores.Max(x => x.Length) : 0;
            cols.Add(new LeadTableColumn(cols.Count + 1, "commonScores", commonScoreMaxLenght, false));

            // вывод турнирной таблицы

            // сначала заголовок
            var separateLine = "+";
            foreach (var col in cols)
            {
                separateLine += GetColValue("", col.Width, '-', true) + "+";
            }
            writer.WriteLine(separateLine);

            //затем каждый игрок

            int playerNumber = 1;
            const string sep = "|";
            foreach (var player in scores.OrderByDescending(x => x.Value["###SUM"]).ThenBy(x => x.Key))
            {
                string line = "";
                // номер+имя
                line = sep +
                    GetColValue(playerNumber.ToString(), cols.First(x => x.Number == 1).Width, ' ', false) + sep +
                    GetColValue(player.Key, cols.Single(x => x.Number == 2).Width, ' ', true) + sep;

                // задачи
                foreach (var task in cols.Where(x => x.IsTask))
                {
                    var playerTaskScore = player.Value.ContainsKey(task.Name) ? player.Value[task.Name] : 0;
                    line += GetColValue(playerTaskScore.ToString(), task.Width, leftAlign: false) + sep;
                }

                // общее кол-во баллов
                var commonPlayerScores = player.Value["###SUM"];
                line += GetColValue(commonPlayerScores.ToString(), cols.Single(x => x.Number == cols.Count).Width, leftAlign: false) + sep;

                writer.WriteLine(line);
                writer.WriteLine(separateLine);

                playerNumber++;
            }
        }

        private static string GetColValue(string value, int lenght, char filler = ' ', bool leftAlign = true)
        {
            var fill = new string(filler, lenght - value.Length);
            return (leftAlign ? "" : fill) + value + (leftAlign ? fill : "");
        }

        private class BrokenTask
        {
            public string Player { get; set; }
            public string Victim { get; set; }
            public string Task { get; set; }
            public bool Result { get; set; }
        }

        private class LeadTableColumn
        {
            public string Name { get; }
            public int Width { get; }
            public int Number { get; }
            public bool IsTask { get; }

            public LeadTableColumn(int number, string name, int width, bool isTask = true)
            {
                Number = number;
                Width = width;
                Name = name;
                IsTask = isTask;
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace YContest
{
    class TaskC
    {
        // Многие программисты любят играть в футбол. Некоторые даже любят проводить свои турниры. Но они не хотят следить за тем, кто сколько очков набрал и какое место занял, 
        // они хотят просто складывать результаты матчей в базу данных, после чего получать турнирную таблицу с количеством набранных очков и итоговым положением.
        // 
        // Одна из таких групп программистов попросила вас помочь им и, пока они собирают команды и проводят свой турнир, написать программу, которая будет строить итоговую турнирную таблицу.
        // Формат ввода
        // Входные данные представляют собой набор строк, каждая строка описывает ровно один сыгранный матч. В каждой строке записаны названия играющих друг с другом команд и результат матча. 
        // Названия и результат разделяются знаком тире, отбитым с обеих сторон пробелами. Каждое название состоит только из латинских букв, начинается с заглавной буквы, все остальные буквы строчные, 
        // гарантируется что длина каждого названия не превосходит 30 символов. Счет записывается в виде A:B, где A — количество голов, забитых первой командой, а B — количество голов, забитых второй командой. 
        // Победившей считается команда, забившая больше голов. Если забито одинаковое количество голов, результатом матча считается ничья. За победу команде присуждается три очка, за ничью — одно, за поражение — ноль.
        // 
        // Гарантируется, что нет ни одной пары команд с одинаковыми названиями, что ни одна пара команд не играла между собой более одного раза. Общее число команд-участников не превосходит 100. Ни в одном матче не было забито больше ста голов.
        // Формат вывода
        // Вам нужно построить турнирную таблицу с результатами.
        // 
        // Каждая строка таблицы — представление результатов каждой из команд, команды должны быть упорядочены в лексикографическом порядке. В первом столбце содержится порядковый номер команды, во втором — название. Далее следуют n столбцов, 
        // в каждом из которых содержится информация об играх с остальными командами: в случае победы в ячейке должна присутствовать буква W, в случае поражения — L, в случае ничьей — D, если участники не играли друг с другом — пробел, 
        // если заполняется ячейка матча игрока с самим собой, то туда следует поставить символ X.
        // 
        // В последних двух столбцах должно быть выписано количество набранных командой очков и итоговое место. Команда A занимает более высокое место, чем команда B, если она набрала большее количество очков, 
        // или они обе набрали одинаковое количество очков, но команда A одержала больше побед, чем команда B. Если же число очков и число побед у команд одинаковое, они занимают одно и то же место. 
        // Для простоты награждения требуется присудить только места с первого по третье.
        // 
        // Все столбцы должны иметь минимально возможную ширину, чтобы вместить данные в каждой строке. Для столбцов, содержащих порядковые номера, количество набранных очков и занятые места, 
        // все данные выравниваются по правому краю, все названия выравниваются по левому краю, после каждого названия в таблице должен гарантированно присутствовать пробел.
        // 
        // Оформляя таблицу, ориентируйтесь на примеры. 
        // 
        // Пример 1
        // Ввод 	
        // 
        // Linux - Gentoo - 1:0
        // Gentoo - Windows - 2:1
        // Linux - Windows - 0:2
        // 
        // Вывод
        // +-+--------+-+-+-+-+-+
        // |1|Gentoo  |X|L|W|3|1|
        // +-+--------+-+-+-+-+-+
        // |2|Linux   |W|X|L|3|1|
        // +-+--------+-+-+-+-+-+
        // |3|Windows |L|W|X|3|1|
        // +-+--------+-+-+-+-+-+
        // 
        // Пример 2
        // Ввод 	
        // 
        // Cplusplus - C - 1:0
        // Cplusplus - Php - 2:0
        // Java - Php - 1:0
        // Java - C - 2:2
        // Java - Perl - 1:1
        // Java - Haskell - 1:1
        // 
        // Вывод
        // +-+----------+-+-+-+-+-+-+-+-+
        // |1|C         |X|L| |D| | |1|3|
        // +-+----------+-+-+-+-+-+-+-+-+
        // |2|Cplusplus |W|X| | | |W|6|1|
        // +-+----------+-+-+-+-+-+-+-+-+
        // |3|Haskell   | | |X|D| | |1|3|
        // +-+----------+-+-+-+-+-+-+-+-+
        // |4|Java      |D| |D|X|D|W|6|2|
        // +-+----------+-+-+-+-+-+-+-+-+
        // |5|Perl      | | | |D|X| |1|3|
        // +-+----------+-+-+-+-+-+-+-+-+
        // |6|Php       | |L| |L| |X|0| |
        // +-+----------+-+-+-+-+-+-+-+-+

        public static void Solve(StreamReader rdr, StreamWriter wr)
        {
            string line;
            while (!rdr.EndOfStream && (line = rdr.ReadLine()) != "")
                AddTeamResult(line);

            CountPlaces();
            ShowTable(wr);
        }

        private static Dictionary<string, Row> Results =
            new Dictionary<string, Row>();

        private static Dictionary<string, int> Places = new Dictionary<string, int>();

        private static int MaxWidth;

        private static void AddTeamResult(string resultLine)
        {
            var splitted = resultLine.Split('-').Select(el => el.Trim()).ToArray();

            if (!Results.ContainsKey(splitted[0]))
                Results.Add(splitted[0], new Row(splitted[0]));
            if (!Results.ContainsKey(splitted[1]))
                Results.Add(splitted[1], new Row(splitted[1]));

            if (splitted[0].Length > MaxWidth)
                MaxWidth = splitted[0].Length;

            if (splitted[1].Length > MaxWidth)
                MaxWidth = splitted[1].Length;

            var count = splitted[2].Split(':').Select(int.Parse).ToArray();

            var teamResult = CountOnSymbol(count[0], count[1]);

            Results[splitted[0]].Results.Add(splitted[1], teamResult);
            Results[splitted[0]].Result.Count += teamResult.Points;
            if (teamResult.Symbol == 'W')
                Results[splitted[0]].Result.WinsCount += 1;

            var otherTeamResult = CountOnSymbol(count[1], count[0]);
            Results[splitted[1]].Results.Add(splitted[0], otherTeamResult);
            Results[splitted[1]].Result.Count += otherTeamResult.Points;
            if (otherTeamResult.Symbol == 'W')
                Results[splitted[1]].Result.WinsCount += 1;
        }

        private static MatchResult CountOnSymbol(int teamA, int teamB)
        {
            return teamA == teamB
                ? new MatchResult('D', 1)
                : teamA > teamB ? new MatchResult('W', 3) : new MatchResult('L', 0);
        }

        private static void CountPlaces()
        {
            SortedDictionary<PlayerResult, List<string>> temp =
                new SortedDictionary<PlayerResult, List<string>>(
                    Comparer<PlayerResult>.Create(
                        (l, r) => (r.Count == l.Count) ? r.WinsCount.CompareTo(l.WinsCount) : r.Count.CompareTo(l.Count)));

            foreach (var result in Results)
            {
                foreach (var resultsKey in Results.Keys)
                    if (!result.Value.Results.ContainsKey(resultsKey))
                        result.Value.Results.Add(resultsKey, new MatchResult(' ', 0));

                if (!temp.ContainsKey(result.Value.Result))
                    temp.Add(result.Value.Result, new List<string> { result.Key });
                else
                    temp[result.Value.Result].Add(result.Key);
            }

            foreach (var winners in temp.Take(3).Select((el, num) => new { el, num }))
                foreach (var winner in winners.el.Value)
                    Places.Add(winner, winners.num + 1);
        }

        private static void ShowTable(StreamWriter wr)
        {
            var separator = TableRowSep();

            wr.WriteLine(separator);

            var sortedKeys = Results.Keys.ToList();
            sortedKeys.Sort();

            foreach (var result in sortedKeys.Select((el, num) => new { el, num }))
            {
                var rowRes = Results[result.el];
                var sortedResultsKeys = rowRes.Results.Keys.ToList();
                sortedResultsKeys.Sort();

                wr.WriteLine(
                    "|" + result.num.ToString().PadLeft(Results.Keys.Count / 10 + 1, ' ') + "|" +
                    result.el.PadRight(MaxWidth, ' ') + " |" +
                    string.Join("|", sortedResultsKeys.Select(k => $"{rowRes.Results[k].Symbol}")) + "|" + rowRes.Result.Count +
                    "|" + (Places.ContainsKey(result.el) ? Places[result.el].ToString() : " ") + "|");
                wr.WriteLine(separator);
            }
        }

        private static string TableRowSep()
        {
            return
                "+" + new string('-', Results.Keys.Count / 10 + 1) + "+" + new string('-', MaxWidth + 1) + "+" +
                string.Concat(Enumerable.Repeat("-+", Results.Keys.Count + 2));
        }
    }

    struct MatchResult
    {
        public MatchResult(char symbol, int points)
        {
            Symbol = symbol;
            Points = points;
        }

        public char Symbol;
        public int Points;
    }

    class Row
    {
        public Row(string owner)
        {
            Results.Add(owner, new MatchResult('X', 0));
        }

        public Dictionary<string, MatchResult> Results { get; set; } = new Dictionary<string, MatchResult>();
        public PlayerResult Result { get; set; } = new PlayerResult();
    }

    class PlayerResult
    {
        public int Count { get; set; }
        public int WinsCount { get; set; }

        private sealed class CountWinsCountEqualityComparer : IEqualityComparer<PlayerResult>
        {
            public bool Equals(PlayerResult x, PlayerResult y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Count == y.Count && x.WinsCount == y.WinsCount;
            }

            public int GetHashCode(PlayerResult obj)
            {
                unchecked
                {
                    return (obj.Count * 397) ^ obj.WinsCount;
                }
            }
        }

        public static IEqualityComparer<PlayerResult> CountWinsCountComparer { get; } =
            new CountWinsCountEqualityComparer();
    }
}

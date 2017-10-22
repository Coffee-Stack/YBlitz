using System;
using System.Diagnostics;
using System.IO;

namespace YContest
{
    class Program
    {
        static void Main(string[] args)
        {
            var rdr = args.Length > 1
                ? new StreamReader(File.OpenRead(args[1]))
                : new StreamReader(Console.OpenStandardInput());

            var wr = args.Length > 2
                ? new StreamWriter(File.OpenRead(args[2]))
                : new StreamWriter(Console.OpenStandardOutput());

            Stopwatch st = Stopwatch.StartNew();
            if (args[0] == "A")
                TaskA.Solve(rdr, wr);
            else if (args[0] == "B")
                TaskB.Solve(rdr, wr);
            else if (args[0] == "C")
                TaskC.Solve(rdr, wr);
            else if (args[0] == "D")
                TaskD.Solve(rdr, wr);
            st.Stop();

            wr.WriteLine();
            wr.WriteLine($"{st.Elapsed}");

            wr.Flush();
        }
    }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace YContest
{
    class Program
    {
        private static readonly bool _showWrongAnswerDetails = false;
        private static readonly bool _showRuntimeErrorDetails = false;
        private static readonly bool _exitAfterError = true;

        static void Main(string[] args)
        {
            var taskName = args[0];

            Action<StreamReader, StreamWriter> run = null;

            if (taskName == "A")
                run = (rdr, wr) => TaskA.Solve(rdr, wr);
            else if (taskName == "B")
                run = (rdr, wr) => TaskB.Solve(rdr, wr);
            else if (taskName == "C")
                run = (rdr, wr) => TaskC.Solve(rdr, wr);
            else if (taskName == "D")
                run = (rdr, wr) => TaskD.Solve(rdr, wr);
            else if (taskName == "C_Sofi")
                run = (rdr, wr) => TaskC_Sofi.Solve(rdr, wr);
            else
            {
                Console.WriteLine("Uncorrect task name");
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
                return;
            }

            var output = SolveTask(taskName, run);

            Console.Write(output);
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static string SolveTask(string taskName, Action<StreamReader, StreamWriter> run)
        {
            var sb = new StringBuilder();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{taskName}");
            foreach (var file in Directory.GetFiles(Path.Combine(path, "In")))
            {                
                var rdr = new StreamReader(file);
                var mem = new MemoryStream();
                var wr = new StreamWriter(mem);

                Stopwatch st = Stopwatch.StartNew();

                try
                {                    
                    run(rdr, wr);
                    st.Stop();

                    wr.Flush();
                                        
                    var result = Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
                    
                    var expectedResult = File.ReadAllText(Path.Combine(path, "Out", Path.GetFileName(file)));

                    var success = string.Equals(expectedResult, result.Trim(), StringComparison.CurrentCulture);
                    
                    sb.AppendLine($"File: {Path.GetFileName(file)}, Result: {(success ? "OK" : "WA")}, Elapsed: {st.ElapsedMilliseconds} ms");

                    if (!success )
                    {
                        if (_showWrongAnswerDetails)
                            sb.Append($"Expected:\n{expectedResult}\nResult:\n{result}\n");
                        if (_exitAfterError)
                            break;
                    }
                }
                catch (Exception ex)
                {
                    st.Stop();
                    sb.AppendLine($"File: {Path.GetFileName(file)}, Result: RE, Elapsed: {st.ElapsedMilliseconds} ms");

                    if (_showRuntimeErrorDetails)
                        sb.Append($"Exception: {ex}\n");

                    if (_exitAfterError)
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
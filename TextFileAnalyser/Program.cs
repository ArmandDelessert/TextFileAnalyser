using System.Reflection;

namespace TextFileAnalyser
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                StartCli(args);
            }
            else
            {
                try
                {
                    PrintHeader();
                    StartInteractiveCli();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"A fatal error occurred and the program will close.{Environment.NewLine}Exception details:{Environment.NewLine}{e}");
                    Console.ResetColor();
                }
                finally
                {
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                }
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("Text file analyser");
            Console.WriteLine($"Version {Assembly.GetExecutingAssembly().GetName().Version}");
        }

        private static void StartCli(string[] args)
        {
            if (args.Length > 0 && Path.Exists(args[0]))
            {
                Analyzer analyzer = new();
                analyzer.AnalyzePath(args[0]);
            }
            else
            {
                Console.WriteLine("You need to provide the path of the file or directory to analyse.");
            }
        }

        private static void StartInteractiveCli()
        {
            var path = UserInterface.AskUserForPath();
            Analyzer analyzer = new();
            analyzer.AnalyzePath(path);
        }
    }
}

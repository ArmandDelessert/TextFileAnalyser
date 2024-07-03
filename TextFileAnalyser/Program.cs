using System.Text;

namespace TextFileAnalyser
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                AnalyzeDirectory(@"C:\Dev\Armand\TextFileAnalyser\TextFileAnalyser");

                if (args.Length > 2 && Path.Exists(args[1]))
                {
                    AnalyzeDirectory(args[1]);
                }
                else
                {
                    Console.WriteLine("You need to provide the path of the directory to analyse.");
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occured: {e.Message}{Environment.NewLine}Exception stack trace:{Environment.NewLine}{e.StackTrace}");
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static void AnalyzeDirectory(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            int totalFiles = files.Length;
            int textFiles = 0;
            Dictionary<string, int> extensionCountDictionary = [];
            Dictionary<string, int> encodingCountDictionary = [];

            foreach (var file in files)
            {
                if (IsTextFile(file))
                {
                    ++textFiles;

                    string extension = Path.GetExtension(file);
                    if (extensionCountDictionary.TryGetValue(extension, out int extensionCount))
                        extensionCountDictionary[extension] = ++extensionCount;
                    else
                        extensionCountDictionary[extension] = 1;

                    var encoding = GetFileEncoding(file);
                    string encodingName = encoding.EncodingName;
                    if (encodingCountDictionary.TryGetValue(encodingName, out int encodingCount))
                        encodingCountDictionary[encodingName] = ++encodingCount;
                    else
                        encodingCountDictionary[encodingName] = 1;

                    AnalyzeTextFile(file);
                }
            }

            Console.WriteLine($"Total files: {totalFiles}");
            Console.WriteLine($"Text files: {textFiles}");
            Console.WriteLine("Files by extension:");
            foreach (var extension in extensionCountDictionary)
            {
                Console.WriteLine($"{extension.Key}: {extension.Value}");
            }
            Console.WriteLine("Files by encoding:");
            foreach (var encoding in encodingCountDictionary)
            {
                Console.WriteLine($"{encoding.Key}: {encoding.Value}");
            }
        }

        private static bool IsTextFile(string filePath)
        {
            string[] textExtensions = { ".txt", ".md", ".html", ".xml", ".json", ".sln", ".csproj", ".cs" };
            string extension = Path.GetExtension(filePath).ToLower();
            return textExtensions.Contains(extension);
        }

        private static Encoding GetFileEncoding(string filePath)
        {
            using var reader = new StreamReader(filePath, true);
            reader.Peek(); // Force the reader to detect the encoding
            return reader.CurrentEncoding;
        }

        private static void AnalyzeTextFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            int spaceCount = 0;
            int tabCount = 0;
            int trailingWhitespaceCount = 0;
            bool hasFinalEmptyLine = false;
            bool hasMixedSpacesAndTabs = false;

            using (var reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    spaceCount += line.Count(c => c == ' ');
                    tabCount += line.Count(c => c == '\t');
                    if (line.EndsWith(' '))
                    {
                        ++trailingWhitespaceCount;
                    }

                    if (line.Contains(' ') && line.Contains('\t'))
                    {
                        hasMixedSpacesAndTabs = true;
                    }
                }

                hasFinalEmptyLine = string.IsNullOrEmpty(line);
            }

            Console.WriteLine($"File: {fileInfo.Name}");
            Console.WriteLine($"Spaces: {spaceCount}");
            Console.WriteLine($"Tabs: {tabCount}");
            Console.WriteLine($"Has mixed spaces and tabs: {hasMixedSpacesAndTabs}");
            Console.WriteLine($"Trailing whitespaces: {trailingWhitespaceCount}");
            Console.WriteLine($"Has final empty line: {hasFinalEmptyLine}");
        }
    }
}

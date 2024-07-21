namespace TextFileAnalyser
{
    internal class UserInterface
    {
        public static string AskUserForPath()
        {
            string? path;
            do
            {
                Console.Write("Please enter the path of the file or folder to be analyzed: ");
                path = Console.ReadLine();

                if (File.Exists(path) || Directory.Exists(path))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("The path entered is invalid. Please try again.");
                }
            } while (true);

            return path;
        }

        public static void ShowResults(int totalFiles, int textFiles, Dictionary<string, int> extensionCountDictionary, Dictionary<string, int> encodingCountDictionary)
        {
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

        public static void ShowFileAnalysis(string fileName, int spaceCount, int tabCount, bool hasMixedSpacesAndTabs, int trailingWhitespaceCount, bool hasFinalEmptyLine)
        {
            Console.WriteLine($"File: {fileName}");
            Console.WriteLine($"Spaces: {spaceCount}");
            Console.WriteLine($"Tabs: {tabCount}");
            Console.WriteLine($"Has mixed spaces and tabs: {hasMixedSpacesAndTabs}");
            Console.WriteLine($"Trailing whitespaces: {trailingWhitespaceCount}");
            Console.WriteLine($"Has final empty line: {hasFinalEmptyLine}");
        }
    }
}

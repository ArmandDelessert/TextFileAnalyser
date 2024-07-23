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

                if (System.IO.File.Exists(path) || Directory.Exists(path))
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

        public static void ShowDirectoryAnalysis(FileSystemContainer fileSystemContainer)
        {
            TraverseDirectory(fileSystemContainer, "  ");
        }

        private static void TraverseDirectory(FileSystemContainer fileSystemContainer, string prefix, string sumPrefix = "")
        {
            ShowFileSystemContainer(fileSystemContainer, prefix);

            foreach (var file in fileSystemContainer.Files)
            {
                ShowFileAnalysis(file, sumPrefix + prefix);
            }

            foreach (var subFileSystemContainer in fileSystemContainer.FileSystemContainers)
            {
                TraverseDirectory(subFileSystemContainer, sumPrefix + prefix, prefix);
            }
        }

        public static void ShowFileSystemContainer(FileSystemContainer fileSystemContainer, string prefix)
        {
            Console.WriteLine($"{prefix}Folder: {fileSystemContainer.FullPath}");
        }

        public static void ShowFileAnalysis(File file, string prefix)
        {
            Console.WriteLine($"{prefix}File: {file.FullPath}");
            Console.WriteLine($"{prefix} Is text file: {file.IsTextFile}");
            Console.WriteLine($"{prefix} Double spaces: {file.DoubleSpaceCount}");
            Console.WriteLine($"{prefix} Tabs: {file.TabCount}");
            Console.WriteLine($"{prefix} Has mixed spaces and tabs: {file.HasMixedSpaceAndTab}");
            Console.WriteLine($"{prefix} CR end line: {file.CrCount}");
            Console.WriteLine($"{prefix} LF end line: {file.LfCount}");
            Console.WriteLine($"{prefix} CRLF end line: {file.CrLfCount}");
            Console.WriteLine($"{prefix} Has mixed end line: {file.HasMixedEndLine}");
            Console.WriteLine($"{prefix} Trailing whitespaces: {file.TrailingWhitespaceCount}");
            Console.WriteLine($"{prefix} Final empty line: {file.FinalEmptyLineCount}");
        }
    }
}

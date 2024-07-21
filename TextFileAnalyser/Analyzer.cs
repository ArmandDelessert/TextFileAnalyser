namespace TextFileAnalyser
{
    internal class Analyzer
    {
        public void AnalyzePath(string path)
        {
            if (!Path.Exists(path))
                throw new ArgumentException("The specified path does not exist.", nameof(path));

            Console.WriteLine($"Analyzing '{path}'...");

            if (File.Exists(path))
            {
                AnalyzeFile(path);
            }
            else if (Directory.Exists(path))
            {
                TraverseDirectory(path);
            }
            else
            {
                Console.WriteLine("The specified path does not exist.");
            }
        }

        private void TraverseDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException("The specified directory does not exist.");

            foreach (var file in Directory.GetFiles(directoryPath))
            {
                AnalyzeFile(file);
            }

            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                TraverseDirectory(dir);
            }
        }

        private void AnalyzeFile(string filePath)
        {
            if (FileUtilities.IsTextFile(filePath))
            {
                AnalyzeTextFile(filePath);
            }
            else
            {
                Console.WriteLine($"Skipping non-text file: {filePath}");
            }
        }

        private void AnalyzeTextFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified file does not exist.");

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

            UserInterface.ShowFileAnalysis(fileInfo.Name, spaceCount, tabCount, hasMixedSpacesAndTabs, trailingWhitespaceCount, hasFinalEmptyLine);
        }
    }
}

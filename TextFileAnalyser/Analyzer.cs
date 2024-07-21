namespace TextFileAnalyser
{
    internal class Analyzer
    {
        public void AnalyzePath(string path)
        {
            Console.WriteLine($"Analyzing '{path}'...");

            if (File.Exists(path))
            {
                AnalyzeFile(path);
            }
            else if (Directory.Exists(path))
            {
                AnalyzeDirectory(path);
            }
            else
            {
                Console.WriteLine("The specified path does not exist.");
            }
        }

        private void AnalyzeDirectoryList(string[] directoryPaths)
        {

        }

        private void AnalyzeDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException("The specified directory does not exist.");

            var files = Directory.GetFiles(directoryPath.Trim());
            AnalyzeTextFileList(files);

            var subDirectories = Directory.GetDirectories(directoryPath);
            AnalyzeDirectoryList(subDirectories);

            //UserInterface.ShowResults(totalFiles, textFiles, extensionCountDictionary, encodingCountDictionary);
        }

        private void AnalyzeTextFileList(string[] filePaths)
        {
            int totalFiles = filePaths.Length;
            int textFiles = 0;
            Dictionary<string, int> extensionCountDictionary = [];
            Dictionary<string, int> encodingCountDictionary = [];

            foreach (var file in filePaths)
            {
                //AnalyzeFile(file);

                if (FileUtilities.IsTextFile(file))
                {
                    ++textFiles;

                    string extension = Path.GetExtension(file);
                    if (extensionCountDictionary.TryGetValue(extension, out int extensionCount))
                        extensionCountDictionary[extension] = ++extensionCount;
                    else
                        extensionCountDictionary[extension] = 1;

                    var encoding = FileUtilities.GetFileEncoding(file);
                    string encodingName = encoding.EncodingName;
                    if (encodingCountDictionary.TryGetValue(encodingName, out int encodingCount))
                        encodingCountDictionary[encodingName] = ++encodingCount;
                    else
                        encodingCountDictionary[encodingName] = 1;

                    AnalyzeTextFile(file);
                }
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

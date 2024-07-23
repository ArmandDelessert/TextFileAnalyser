namespace TextFileAnalyser
{
    internal class Analyzer
    {
        public void AnalyzePath(string path)
        {
            if (!Path.Exists(path))
                throw new ArgumentException("The specified path does not exist.", nameof(path));

            path = path.Trim();

            Console.WriteLine($"Analyzing '{path}'...");

            if (System.IO.File.Exists(path))
            {
                File file = AnalyzeFile(path);
                UserInterface.ShowFileAnalysis(file, "  ");
            }
            else if (Directory.Exists(path))
            {
                FileSystemContainer fileSystemContainer = TraverseDirectory(path);
                UserInterface.ShowDirectoryAnalysis(fileSystemContainer);
            }
            else
            {
                Console.WriteLine("The specified path does not exist.");
            }
        }

        private FileSystemContainer TraverseDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException("The specified directory does not exist.");

            FileSystemContainer fileSystemContainer = new(directoryPath);

            foreach (var file in Directory.GetFiles(directoryPath.Trim()))
            {
                fileSystemContainer.Add(AnalyzeFile(file));
            }

            foreach (var directory in Directory.GetDirectories(directoryPath.Trim()))
            {
                fileSystemContainer.Add(TraverseDirectory(directory));
            }

            return fileSystemContainer;
        }

        private File AnalyzeFile(string filePath)
        {
            if (FileUtilities.IsTextFile(filePath))
            {
                return AnalyzeTextFile(filePath);
            }
            else
            {
                Console.WriteLine($"Skipping non-text file: {filePath}");
                return new File(filePath, false);
            }
        }

        private File AnalyzeTextFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("The specified file does not exist.");

            var file = new File(filePath, true);

            using (var reader = new StreamReader(filePath))
            {
                string? line;
                int emptyLineCount = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == string.Empty)
                    {
                        emptyLineCount++;
                    }
                    else
                    {
                        emptyLineCount = 0;
                    }

                    file.DoubleSpaceCount += CountDoubleSpaces(line);
                    file.TabCount += line.Count(c => c == '\t');

                    if (line.EndsWith(' ') || line.EndsWith('\t'))
                    {
                        ++file.TrailingWhitespaceCount;
                    }

                    file.CrCount += CountEndings(line, '\r');
                    file.LfCount += CountEndings(line, '\n');
                    file.CrLfCount += CountEndings(line, "\r\n");
                }

                file.FinalEmptyLineCount = emptyLineCount;
            }

            return file;
        }

        private static int CountDoubleSpaces(string line)
        {
            int count = 0;
            for (int i = 0; i < line.Length - 1; i++)
            {
                if (line[i] == ' ' && line[i + 1] == ' ')
                {
                    count++;
                    i++; // On évite de recompter le second espace.
                }
            }
            return count;
        }

        private static int CountEndings(string line, char ending)
        {
            int count = 0;
            foreach (var c in line)
            {
                if (c == ending)
                {
                    count++;
                }
            }
            return count;
        }

        private static int CountEndings(string line, string ending)
        {
            int count = 0;
            int index = 0;
            while ((index = line.IndexOf(ending, index)) != -1)
            {
                count++;
                index += ending.Length;
            }
            return count;
        }
    }
}

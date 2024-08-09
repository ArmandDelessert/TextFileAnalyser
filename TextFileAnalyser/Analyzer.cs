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
                Folder folder = TraverseDirectory(path);
                UserInterface.ShowDirectoryAnalysis(folder);
            }
            else
            {
                Console.WriteLine("The specified path does not exist.");
            }
        }

        private Folder TraverseDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException("The specified directory does not exist.");

            Folder folder = new(directoryPath);

            foreach (var file in Directory.GetFiles(directoryPath.Trim()))
            {
                folder.Add(AnalyzeFile(file));
            }

            foreach (var directory in Directory.GetDirectories(directoryPath.Trim()))
            {
                folder.Add(TraverseDirectory(directory));
            }

            return folder;
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

        // Test fonction de ChatGPT
        private File AnalyzeTextFileCharByChar(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("The specified file does not exist.");

            var file = new File(filePath, true);

            using (var reader = new StreamReader(filePath))
            {
                int lineCount = 0;
                int emptyLineCount = 0;
                int spaceCount = 0;
                int doubleSpaceCount = 0;
                int tabCount = 0;
                int crCount = 0;
                int lfCount = 0;
                int crlfCount = 0;
                int trailingWhitespaceCount = 0;
                bool previousWasCR = false;
                bool lineEmpty = true;

                int charRead;
                char[] previousChars = new char[4];
                while ((charRead = reader.Read()) != -1)
                {
                    char c = (char)charRead;

                    if (c == ' ')
                    {
                        spaceCount++;
                        if (previousChars == ' ')
                        {
                            doubleSpaceCount++;
                        }
                        lineEmpty = false;
                    }
                    else if (c == '\t')
                    {
                        tabCount++;
                        lineEmpty = false;
                    }
                    else if (c == '\r')
                    {
                        crCount++;
                        previousWasCR = true;
                        if (lineEmpty)
                        {
                            emptyLineCount++;
                        }
                        else
                        {
                            emptyLineCount = 0;
                        }
                        lineEmpty = true;
                    }
                    else if (c == '\n')
                    {
                        if (previousWasCR)
                        {
                            crlfCount++;
                            crCount--; // Adjust the previous CR count because it is part of CRLF
                        }
                        else
                        {
                            lfCount++;
                        }
                        if (lineEmpty)
                        {
                            emptyLineCount++;
                        }
                        else
                        {
                            emptyLineCount = 0;
                        }
                        lineEmpty = true;
                        previousWasCR = false;
                    }
                    else
                    {
                        lineEmpty = false;
                        previousWasCR = false;
                        spaceCount = 0;
                    }

                    // Check for trailing whitespace at the end of a line
                    if (c == '\r' || c == '\n')
                    {
                        if (spaceCount > 0 || tabCount > 0)
                        {
                            trailingWhitespaceCount++;
                        }
                        spaceCount = 0;
                        tabCount = 0;
                    }

                    previousChars += c;
                }

                file.LineCount = lineCount;
                file.DoubleSpaceCount = doubleSpaceCount;
                file.TabCount = tabCount;
                file.CrCount = crCount;
                file.LfCount = lfCount;
                file.CrLfCount = crlfCount;
                file.TrailingWhitespaceCount = trailingWhitespaceCount;
                file.FinalEmptyLineCount = emptyLineCount;
            }

            return file;
        }
    }
}

namespace TextFileAnalyser
{
    internal class Analyzer
    {
        private const int WindowSize = 4;
        private AnalyzerWindow Window = new(WindowSize);

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
                    file.TotalTabCount += line.Count(c => c == '\t');

                    if (line.EndsWith(' ') || line.EndsWith('\t'))
                    {
                        ++file.LineWithTrailingWhitespaceCount;
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
                // Final stats
                int charCount = 0;
                int lineCount = 0;
                int totalSpaceCount = 0;
                int doubleSpaceCount = 0;
                int totalTabCount = 0;
                int crCount = 0;
                int lfCount = 0;
                int crlfCount = 0;
                int lineWithTrailingWhitespaceCount = 0;
                int totalEmptyLineCount = 0;
                int finalEmptyLineCount = 0;

                // Temporary stats
                bool lineEmpty = true;

                int charRead;
                while ((charRead = reader.Read()) != -1)
                {
                    charCount++;
                    Window.AddChar((char)charRead);

                    // Comptage des espaces
                    if (Window.GetChar() == ' ')
                    {
                        totalSpaceCount++;
                        if (Window.GetChar(1) == ' ') // TODO : Ça marche pas ça. Il faut corriger.
                        {
                            doubleSpaceCount++;
                        }
                        lineEmpty = false;
                    }
                    // Comptage des tabulations
                    else if (Window.GetChar() == '\t')
                    {
                        totalTabCount++;
                        lineEmpty = false;
                    }
                    // Comptage des retour à la ligne (Carriage Return (CR))
                    else if (Window.GetChar() == '\r')
                    {
                        crCount++;
                        if (lineEmpty)
                        {
                            finalEmptyLineCount++;
                        }
                        else
                        {
                            finalEmptyLineCount = 0;
                        }
                        lineEmpty = true;
                    }
                    // Comptage des retours à la ligne (Line Feed (LF))
                    else if (Window.GetChar() == '\n')
                    {
                        if (Window.GetChar(1) == '\r')
                        {
                            crlfCount++;
                            crCount--; // Adjust the previous CR count because it is part of CRLF.
                        }
                        else
                        {
                            lfCount++;
                        }
                        if (lineEmpty)
                        {
                            finalEmptyLineCount++;
                        }
                        else
                        {
                            finalEmptyLineCount = 0;
                        }
                        lineEmpty = true;
                    }
                    // Autre caractère
                    else
                    {
                        lineEmpty = false;
                    }

                    // Check for trailing whitespace at the end of a line
                    if (Window.GetChar() == '\r' || Window.GetChar() == '\n')
                    {
                        if (Window.GetChar(1) == ' ' || Window.GetChar(1) == '\t')
                        {
                            lineWithTrailingWhitespaceCount++;
                        }
                    }
                }

                file.CharCount = charCount;
                file.LineCount = lineCount;
                file.TotalSpaceCount = totalSpaceCount;
                file.DoubleSpaceCount = doubleSpaceCount;
                file.TotalTabCount = totalTabCount;
                file.CrCount = crCount;
                file.LfCount = lfCount;
                file.CrLfCount = crlfCount;
                file.LineWithTrailingWhitespaceCount = lineWithTrailingWhitespaceCount;
                file.TotalEmptyLineCount = totalEmptyLineCount;
                file.FinalEmptyLineCount = finalEmptyLineCount;
            }

            return file;
        }
    }
}

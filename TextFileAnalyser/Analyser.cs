namespace TextFileAnalyser;

public class Analyser
{
    private const int WindowSize = 4;
    private readonly AnalyserWindow Window = new(WindowSize);

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

    public Folder TraverseDirectory(string directoryPath)
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

    public File AnalyzeFile(string filePath)
    {
        if (FileUtilities.IsTextFile(filePath))
        {
            //return AnalyzeTextFile(filePath);
            return AnalyzeTextFileCharByChar(filePath);
        }
        else
        {
            Console.WriteLine($"Skipping non-text file: {filePath}");
            return new File(filePath, false);
        }
    }

    public File AnalyzeTextFile(string filePath)
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

    public static int CountDoubleSpaces(string line)
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

    public static int CountEndings(string line, char ending)
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

    public static int CountEndings(string line, string ending)
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

    public File AnalyzeTextFileCharByChar(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
            throw new FileNotFoundException("The specified file does not exist.");

        var file = new File(filePath, true);

        using var reader = new StreamReader(filePath);
        return AnalyzeStreamCharByChar(reader, file);
    }

    public File AnalyzeStreamCharByChar(TextReader reader, File file)
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
        bool lineEmpty = true; // Ligne vide ou avec uniquement des espaces blancs.

        int charRead;
        while ((charRead = reader.Read()) != -1)
        {
            charCount++;
            Window.AddChar((char)charRead);

            // Comptage des caractères blancs
            if (Char.IsWhiteSpace(Window.GetChar()))
            {
                // Comptage des espaces
                if (Window.GetChar() == ' ')
                {
                    totalSpaceCount++;
                    if (Window.GetChar(1) == ' ') // TODO : Ça marche pas ça. Il faut corriger.
                    {
                        doubleSpaceCount++;
                    }
                }
                // Comptage des tabulations
                else if (Window.GetChar() == '\t')
                {
                    totalTabCount++;
                }
                // Comptage des retour à la ligne (Carriage Return (CR))
                else if (Window.GetChar() == '\r')
                {
                    crCount++;
                    lineCount++;

                    // Vérification de la présence d'espaces blancs en fin de ligne.
                    if (IsWhiteSpaceButNotNewLine(Window.GetChar(1)))
                    {
                        lineWithTrailingWhitespaceCount++;
                    }

                    if (lineEmpty)
                    {
                        totalEmptyLineCount++;
                        finalEmptyLineCount++;
                    }
                    else
                    {
                        finalEmptyLineCount = 0;
                    }
                    lineEmpty = true; // Réinistialisation de l'indicateur.
                }
                // Comptage des retours à la ligne (Line Feed (LF))
                else if (Window.GetChar() == '\n')
                {
                    if (Window.GetChar(1) == '\r')
                    {
                        crlfCount++;
                        crCount--; // Retire le CR précédent du comptage car il fait partie du CRLF.

                        if (lineEmpty)
                        {
                            // TODO : Pourquoi ?
                            totalEmptyLineCount--;
                            finalEmptyLineCount--;
                        }
                    }
                    else
                    {
                        lfCount++;
                        lineCount++;

                        // Vérification de la présence d'espaces blancs en fin de ligne.
                        if (IsWhiteSpaceButNotNewLine(Window.GetChar(1)))
                        {
                            lineWithTrailingWhitespaceCount++;
                        }
                    }

                    if (lineEmpty)
                    {
                        totalEmptyLineCount++;
                        finalEmptyLineCount++;
                    }
                    else
                    {
                        finalEmptyLineCount = 0;
                    }
                    lineEmpty = true; // Réinistialisation de l'indicateur.
                }
                else
                {
                    // TODO : Compter le nombre de caractères blancs autres ?
                }
            }
            // Autre caractère
            else
            {
                // TODO : Compter le nombre d'autres caractères non-blancs ?
                lineEmpty = false;
            }
        }

        // Caractère blanc
        if (Char.IsWhiteSpace(Window.GetChar()))
        {
            // Retour à la ligne
            if (IsNewLine(Window.GetChar()))
            {
                lineCount++;
                totalEmptyLineCount++;
                finalEmptyLineCount++;
            }
            else
            {
                lineWithTrailingWhitespaceCount++;

                if (lineEmpty)
                {
                    totalEmptyLineCount++;
                    finalEmptyLineCount++;
                }
            }
        }
        // Autre caractère
        else
        {
            finalEmptyLineCount = 0;
        }

        // Le fichier comporte 1 ligne s'il n'est pas vide et qu'il ne comporte aucun retour à la ligne.
        if (lineCount == 0 && charCount > 0)
            lineCount++;

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

        return file;
    }

    private static bool IsNewLine(char c)
        => c == '\r' || c == '\n';

    private static bool IsWhiteSpaceButNotNewLine(char c)
        => Char.IsWhiteSpace(c) && !IsNewLine(c);
}

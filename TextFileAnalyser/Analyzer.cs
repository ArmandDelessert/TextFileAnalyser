namespace TextFileAnalyser
{
    internal class Analyzer
    {
        public void AnalyzePath(string path)
        {
            if (!Path.Exists(path))
                throw new ArgumentException("The specified path does not exist.", nameof(path));

            Console.WriteLine($"Analyzing '{path}'...");

            if (System.IO.File.Exists(path))
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

        private FileSystemContainer TraverseDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException("The specified directory does not exist.");

            FileSystemContainer fileSystemContainer = new(directoryPath);

            foreach (var file in Directory.GetFiles(directoryPath))
            {
                fileSystemContainer.Add(AnalyzeFile(file));
            }

            foreach (var directory in Directory.GetDirectories(directoryPath))
            {
                fileSystemContainer.Add(TraverseDirectory(directory));
            }

            return fileSystemContainer;
        }

        // TODO ADT : Supprimer cette méthode.
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

        private File AnalyzeFile(string filePath)
        {
            if (FileUtilities.IsTextFile(filePath))
            {
                return AnalyzeTextFile(filePath);
            }
            else
            {
                Console.WriteLine($"Skipping non-text file: {filePath}");
                return new File(filePath);
            }
        }

        private File AnalyzeTextFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("The specified file does not exist.");

            var file = new File(filePath);

            using (var reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    file.DoubleSpaceCount += line.Count(c => c == ' '); // TODO ADT : Compter les doubles espaces seulement.
                    file.TabCount += line.Count(c => c == '\t');
                    if (line.EndsWith(' ') || line.EndsWith('\t'))
                    {
                        ++file.TrailingWhitespaceCount;
                    }
                }

                file.HasFinalEmptyLine = string.IsNullOrEmpty(line);
            }

            return file;
        }
    }
}

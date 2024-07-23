namespace TextFileAnalyser
{
    internal class FileSystemContainer
    {
        public FileSystemContainer(string fullPath)
        {
            if (!Directory.Exists(fullPath))
                throw new DirectoryNotFoundException("The specified directory does not exist.");

            FullPath = fullPath;
            Name = Path.GetFileName(fullPath) ?? "[Root]";

            Files = [];
            FileSystemContainers = [];
        }

        public string Name { get; set; }
        public string FullPath { get; set; }

        public List<File> Files { get; }
        public List<FileSystemContainer> FileSystemContainers { get; }

        public void Add(File file) => Files.Add(file);
        public void Add(FileSystemContainer fileSystemContainer) => FileSystemContainers.Add(fileSystemContainer);
    }
}

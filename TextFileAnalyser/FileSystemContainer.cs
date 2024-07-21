namespace TextFileAnalyser
{
    internal class FileSystemContainer
    {
        public FileSystemContainer(string fullPath)
        {
            Name = System.IO.Path.GetDirectoryName(fullPath) ?? "[Root]";
            Path = Directory.GetParent(fullPath)?.FullName ?? string.Empty;

            Files = [];
            FileSystemContainers = [];
        }

        public required string Name { get; set; }

        public required string Path { get; set; }

        public string FullPath => System.IO.Path.Combine(Path, Name);

        public List<File> Files { get; }

        public List<FileSystemContainer> FileSystemContainers { get; }
    }
}

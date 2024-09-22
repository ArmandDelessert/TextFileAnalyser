namespace TextFileAnalyser;

public class Folder
{
    public Folder(string fullPath)
    {
        if (!Directory.Exists(fullPath))
            throw new DirectoryNotFoundException("The specified directory does not exist.");

        FullPath = fullPath;
        Name = Path.GetFileName(fullPath) ?? "[Root]";

        Files = [];
        Folders = [];
    }

    public string Name { get; set; }
    public string FullPath { get; set; }

    public List<File> Files { get; }
    public List<Folder> Folders { get; }

    public void Add(File file) => Files.Add(file);
    public void Add(Folder folder) => Folders.Add(folder);
}

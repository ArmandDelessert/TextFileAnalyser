using System.Text;

namespace TextFileAnalyser
{
    internal class FileUtilities
    {
        public static bool IsTextFile(string filePath)
        {
            string[] textExtensions = [".txt", ".md", ".html", ".xml", ".json", ".sln", ".csproj", ".cs"];
            string extension = Path.GetExtension(filePath).ToLower();
            return textExtensions.Contains(extension);
        }

        public static Encoding GetFileEncoding(string filePath)
        {
            using var reader = new StreamReader(filePath, true);
            reader.Peek(); // Force the reader to detect the encoding.
            return reader.CurrentEncoding;
        }
    }
}

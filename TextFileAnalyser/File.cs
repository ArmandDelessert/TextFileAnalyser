namespace TextFileAnalyser
{
    internal struct File
    {
        public File(string fullPath)
        {
            if (!System.IO.File.Exists(fullPath))
                throw new FileNotFoundException("The specified file does not exist.");

            Name = System.IO.Path.GetDirectoryName(fullPath) ?? "[Root]";
            Extension = System.IO.Path.GetExtension(fullPath) ?? string.Empty;
            Path = Directory.GetParent(fullPath)?.FullName ?? string.Empty;
        }

        public string Name { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }

        public bool IsTextFile { get; set; }
        public int SpaceCount { get; set; }
        public int TabCount { get; set; }
        public int SpaceCountForATab { get; set; }
        public readonly bool HasMixedSpaceAndTab => SpaceCount > 0 && TabCount > 0;
        public int TrailingWhitespaceCount { get; set; }
        public bool HasFinalEmptyLine { get; set; }
    }
}

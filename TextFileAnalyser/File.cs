namespace TextFileAnalyser
{
    internal record File
    {
        public required string Name { get; set; }
        public required string Extension { get; set; }
        public required string Path { get; set; }

        public bool IsTextFile { get; set; }
        public int SpaceCount { get; set; }
        public int TabCount { get; set; }
        public bool FinalEmptyLine { get; set; }
    }
}

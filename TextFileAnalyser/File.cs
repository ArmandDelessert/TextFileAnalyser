namespace TextFileAnalyser
{
    internal struct File
    {
        public File(string fullPath)
        {
            if (!System.IO.File.Exists(fullPath))
                throw new FileNotFoundException("The specified file does not exist.");

            FullPath = fullPath;
            Name = Path.GetFileName(fullPath) ?? "[Root]";
            Extension = Path.GetExtension(fullPath) ?? string.Empty;
        }

        public File(string fullPath, bool isTextFile) : this(fullPath)
        {
            IsTextFile = isTextFile;
        }

        public string Name { get; set; }
        public string Extension { get; set; }
        public string FullPath { get; set; }

        public byte[] Hash /*{ get; private set; }*/ => throw new NotImplementedException();
        public DateTime HashComputationDate /*{ get; private set; }*/ => throw new NotImplementedException();
        public bool IsTextFile { get; set; }

        public int CharCount { get; set; }
        public int LineCount { get; set; }

        public int DoubleSpaceCount { get; set; }
        public int TabCount { get; set; }
        public int SpaceCountForATab { get; set; }
        public readonly bool HasMixedSpaceAndTab => DoubleSpaceCount > 0 && TabCount > 0;

        public int CrCount { get; set; }
        public int LfCount { get; set; }
        public int CrLfCount { get; set; }
        public readonly bool HasMixedEndLine =>
            CrCount > 0 && LfCount > 0
            || CrCount > 0 && CrLfCount > 0
            || LfCount > 0 && CrLfCount > 0;

        public int TrailingWhitespaceCount { get; set; }
        public int FinalEmptyLineCount { get; set; }

        public void ComputeHash() { throw new NotImplementedException(); }
    }
}

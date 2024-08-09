namespace TextFileAnalyser
{
    internal class AnalyzerWindow(int windowSize)
    {
        public char[] Window { get; set; } = new char[windowSize];

        public int WindowSize { get; set; } = windowSize;

        public int WindowIndex { get; set; } = 0;

        public void AddChar(char c)
        {
            Window[WindowIndex] = c;
            MoveWindowIndex();
        }

        public char[] GetWindow() => Window;

        private void MoveWindowIndex()
        {
            if (WindowIndex < WindowSize - 1)
                WindowIndex++;
            else
                WindowIndex = 0;
        }
    }
}

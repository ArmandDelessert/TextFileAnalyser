namespace TextFileAnalyser
{
    internal class AnalyzerWindow(int windowSize)
    {
        public char[] Window { get; private set; } = new char[windowSize];

        private int CurrentCharIndex => MoveIndex(NextCharIndex, -1);

        private int NextCharIndex { get; set; } = 0;

        public void AddChar(char c)
        {
            Window[NextCharIndex] = c;
            IncreaseNextCharIndex();
        }

        public char GetChar(int index = 0) => Window[MoveIndex(CurrentCharIndex, -index)];

        public char[] GetWindow() => Window;

        private void IncreaseNextCharIndex() => NextCharIndex = MoveIndex(NextCharIndex);

        private int MoveIndex(int index, int increment = 1)
        {
            return (index + increment) % Window.Length;
        }
    }
}

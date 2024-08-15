using TextFileAnalyser.Test;

namespace TextFileAnalyser
{
    public class AnalyserWindow(int windowSize)
    {
        public char[] Window { get; private set; } = new char[windowSize];

        private RollingIndex CurrentCharIndex { get; set; } = new RollingIndex(0, windowSize - 1);

        private int NextCharIndex => CurrentCharIndex.GetNextIndex();

        public void AddChar(char c)
        {
            Window[NextCharIndex] = c;
            IncreaseWindowIndex();
        }

        public char GetChar(int index = 0) => Window[CurrentCharIndex.GetNextIndex(-index)];

        public char[] GetWindow() => Window;

        private void IncreaseWindowIndex() => CurrentCharIndex.Increase();
    }
}

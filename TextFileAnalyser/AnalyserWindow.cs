using TextFileAnalyser.Test;

namespace TextFileAnalyser
{
    public class AnalyserWindow(int windowSize)
    {
        public char[] Window { get; private set; } = new char[windowSize];

        public int CharAdded { get; private set; } = 0;

        private RollingIndex CurrentCharIndex { get; set; } = new RollingIndex(0, windowSize - 1);

        private int NextCharIndex => CurrentCharIndex.GetNextIndex();

        public void AddChar(char c)
        {
            Window[NextCharIndex] = c;
            CharAdded++;
            IncreaseWindowIndex();
        }

        public char GetChar(int index = 0) => Window[CurrentCharIndex.GetNextIndex(-index)];

        public IList<char> GetWindow()
        {
            // cas 1 : Aucun char ajouté.
            if (CharAdded == 0)
                return [];

            // Cas 1 : Tableau déjà ordré.
            if (NextCharIndex == 0)
                return Window;

            // Cas 2 : Début du remplissage du tableau (sans réécriture).
            if (CharAdded < Window.Length)
                return Window.Take(CurrentCharIndex.Index + 1).ToList();

            // Cas 3 : Remplissage du talbeau avec réécriture.
            return Window.Skip(NextCharIndex).Take(windowSize).ToList();
        }

        private void IncreaseWindowIndex() => CurrentCharIndex.Increase();
    }
}

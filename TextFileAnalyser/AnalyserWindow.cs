using TextFileAnalyser.Test;

namespace TextFileAnalyser;

public class AnalyserWindow(int windowSize)
{
    public char[] Window { get; private set; } = new char[windowSize];

    public int CharAdded { get; private set; } = 0;

    private RollingIndex CurrentCharIndex { get; set; } = new RollingIndex(0, windowSize - 1, 0);

    private RollingIndex NextCharIndex { get; set; } = new RollingIndex(0, windowSize - 1, 0);

    public void AddChar(char c)
    {
        Window[NextCharIndex.Index] = c;
        if (CharAdded++ > 0)
            CurrentCharIndex.Increase(); // On n'incrémente pas l'index s'il s'agit du premier caractère ajouté.
        NextCharIndex.Increase();
    }

    public char GetChar(int index = 0) => Window[CurrentCharIndex.GetNextIndex(-index)];

    public IList<char> GetWindow()
    {
        // Cas 1 : Aucun caractère ajouté pour l'instant.
        if (CharAdded == 0)
            return [];

        // Cas 2 : Tableau déjà ordré ('NextCharIndex' pointe le début du tableau).
        if (NextCharIndex.Index == 0)
            return Window.ToList();

        // Cas 3 : Début du remplissage du tableau (sans réécriture). 'NextCharIndex' ne doit pas être pris en compte.
        if (CharAdded < Window.Length)
            return Window.Take(CurrentCharIndex.Index + 1).ToList();

        // Cas 4 : Remplissage du talbeau avec réécriture.
        var firstPart = Window.Skip(NextCharIndex.Index).Take(windowSize - NextCharIndex.Index).ToList();
        var secondPart = Window.Take(CurrentCharIndex.Index + 1).ToList();
        return [.. firstPart, .. secondPart];
    }
}

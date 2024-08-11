namespace TextFileAnalyser.Test
{
    public class RollingIndex
    {
        public RollingIndex(int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
                throw new ArgumentException("La valeur minimale doit être plus petite que la valeur maximale.");

            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Index = MinIndex;
        }

        public int MinIndex { get; }

        public int MaxIndex { get; }

        public int Range => MaxIndex - MinIndex;

        public int Index { get; private set; }

        public void Increase(int increment = 1)
        {
            Index += increment;

            if (Index < MinIndex)
            {
                Index = ;
            }
            if (Index > MaxIndex)
            {
                Index = ;
            }
        }
    }
}

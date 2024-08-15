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

        public int Range => MaxIndex - MinIndex + 1;

        public int Index { get; private set; }

        public int UpRollingCount { get; private set; } = 0;

        public int DownRollingCount { get; private set; } = 0;

        public void Increase(int increment = 1)
        {
            var result = DumbIncreaseIndex(Index, UpRollingCount, DownRollingCount, increment);
            Index = result.Index;
            UpRollingCount = result.UpRolingCount;
            DownRollingCount = result.DownRollingCount;
        }

        public int GetNextIndex(int increment = 1) => DumbIncreaseIndex(Index, UpRollingCount, DownRollingCount, increment).Index;

        private (int Index, int UpRolingCount , int DownRollingCount) DumbIncreaseIndex(int index, int upRollingCount, int downRollingCount, int increment)
        {
            if (increment > 0)
            {
                while (increment > 0)
                {
                    index++;
                    increment--;

                    if (index > MaxIndex)
                    {
                        index = MinIndex;
                        upRollingCount++;
                    }
                }
            }
            else if (increment < 0)
            {
                while (increment < 0)
                {
                    index--;
                    increment++;

                    if (index < MinIndex)
                    {
                        index = MaxIndex;
                        downRollingCount++;
                    }
                }
            }

            return (index, upRollingCount, downRollingCount);
        }

        private void SmartIncrease(int increment)
        {
            // TODO : Finish, test and validate.

            Index += increment;

            if (Index < MinIndex)
            {
                Index = MinIndex; // TODO : Pas juste, à corriger.
                DownRollingCount++; // TODO : On peut faire plusieurs tours.
            }
            if (Index > MaxIndex)
            {
                Index = MinIndex; // TODO : Pas juste, à corriger.
                UpRollingCount++; // TODO : On peut faire plusieurs tours.
            }
        }
    }
}

namespace BattleCruisers.Utils.DataStrctures
{
    public class Range<T> : IRange<T>
    {
        public T Min { get; private set; }
        public T Max { get; private set; }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }
    }
}

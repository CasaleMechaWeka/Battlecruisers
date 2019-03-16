namespace BattleCruisers.Utils.DataStrctures
{
    public class Range<T> : IRange<T>
    {
        public T Min { get; }
        public T Max { get; }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public override bool Equals(object obj)
        {
            Range<T> other = obj as Range<T>;
            return
                other != null
                && other.Min.Equals(Min)
                && other.Max.Equals(Max);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Min, Max);
        }
    }
}

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

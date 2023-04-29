namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures
{
    // PERF  Struct candidate :)
    public class PvPRange<T> : IPvPRange<T>
    {
        public T Min { get; }
        public T Max { get; }

        public PvPRange(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public override bool Equals(object obj)
        {
            PvPRange<T> other = obj as PvPRange<T>;
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

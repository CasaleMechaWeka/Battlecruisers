using BattleCruisers.Utils;

namespace BattleCruisers.UI.Filters
{
    public class StaticFilter<TElement> : IFilter<TElement>
    {
        private readonly bool _isMatch;

        public StaticFilter(bool isMatch)
        {
            _isMatch = isMatch;
        }

        public bool IsMatch(TElement element)
        {
            return _isMatch;
        }
    }
}
using System;

namespace BattleCruisers.UI
{
    public class StaticFilter<TElement> : IBroadcastingFilter<TElement>
    {
        private readonly bool _isMatch;

#pragma warning disable 67  // Unused event
        public event EventHandler PotentialMatchChange;
#pragma warning restore 67  // Unused event

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

using System;

namespace BattleCruisers.UI
{
    public class StaticDecider<TElement> : IFilter<TElement>
    {
        private readonly bool _shouldBeEnabled;

#pragma warning disable 67  // Unused event
        public event EventHandler PotentialMatchChange;
#pragma warning restore 67  // Unused event

        public StaticDecider(bool shouldBeEnabled)
        {
            _shouldBeEnabled = shouldBeEnabled;
        }

        public bool IsMatch(TElement element)
        {
            return _shouldBeEnabled;
        }
    }
}

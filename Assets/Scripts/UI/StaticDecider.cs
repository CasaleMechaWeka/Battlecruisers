using System;

namespace BattleCruisers.UI
{
    public class StaticDecider<TElement> : IActivenessDecider<TElement>
    {
        private readonly bool _shouldBeEnabled;

#pragma warning disable 67  // Unused event
        public event EventHandler PotentialActivenessChange;
#pragma warning restore 67  // Unused event

        public StaticDecider(bool shouldBeEnabled)
        {
            _shouldBeEnabled = shouldBeEnabled;
        }

        public bool ShouldBeEnabled(TElement element)
        {
            return _shouldBeEnabled;
        }
    }
}

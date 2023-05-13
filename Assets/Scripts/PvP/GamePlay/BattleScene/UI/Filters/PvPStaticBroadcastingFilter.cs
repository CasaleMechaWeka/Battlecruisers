using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters
{
    public class PvPStaticBroadcastingFilter : IPvPBroadcastingFilter
    {
        public bool IsMatch { get; }

#pragma warning disable 67  // Unused event
        public event EventHandler PotentialMatchChange;
#pragma warning restore 67  // Unused event

        public PvPStaticBroadcastingFilter(bool isMatch)
        {
            IsMatch = isMatch;
        }
    }

    public class StaticBroadcastingFilter<TElement> : IPvPBroadcastingFilter<TElement>
    {
        private readonly bool _isMatch;

#pragma warning disable 67  // Unused event
        public event EventHandler PotentialMatchChange;
#pragma warning restore 67  // Unused event

        public StaticBroadcastingFilter(bool isMatch)
        {
            _isMatch = isMatch;
        }

        public bool IsMatch(TElement element)
        {
            return _isMatch;
        }
    }
}

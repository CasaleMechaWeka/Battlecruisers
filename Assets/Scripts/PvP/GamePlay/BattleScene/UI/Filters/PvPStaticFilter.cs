using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters
{
    public class PvPStaticFilter<TElement> : IPvPFilter<TElement>
    {
        private readonly bool _isMatch;

        public PvPStaticFilter(bool isMatch)
        {
            _isMatch = isMatch;
        }

        public bool IsMatch(TElement element)
        {
            return _isMatch;
        }

        public bool IsMatch(TElement element, VariantPrefab variant)
        {
            return _isMatch;
        }
    }
}
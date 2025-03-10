using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters
{
    public class PvPStaticFilter<TElement> : IFilter<TElement>
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
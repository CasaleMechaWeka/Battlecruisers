using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

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
    }
}
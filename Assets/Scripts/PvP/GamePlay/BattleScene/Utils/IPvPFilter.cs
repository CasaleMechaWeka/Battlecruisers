using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils
{
    public interface IPvPFilter<TElement>
    {
        bool IsMatch(TElement element);
        bool IsMatch(TElement element, VariantPrefab variant);
    }
}

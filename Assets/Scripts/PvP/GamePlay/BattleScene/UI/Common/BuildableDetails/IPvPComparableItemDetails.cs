using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.LoadoutScreen.Comparisons;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public interface IPvPComparableItemDetails<TItem> : IPvPDismissableEmitter where TItem : IPvPComparableItem
    {
        void ShowItemDetails(TItem item, TItem itemToCompareTo = default);
        void ShowItemDetails(TItem item, VariantPrefab variant, TItem itemToCompareTo = default);
        void Hide();
        PvPBuildingVariantDetailController GetBuildingVariantDetailController();
        PvPUnitVariantDetailController GetUnitVariantDetailController();
    }
}

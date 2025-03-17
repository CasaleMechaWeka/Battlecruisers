using BattleCruisers.UI;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public interface IPvPComparableItemDetails<TItem> : IDismissableEmitter where TItem : IComparableItem
    {
        void ShowItemDetails(TItem item, TItem itemToCompareTo = default);
        void ShowItemDetails(TItem item, VariantPrefab variant, TItem itemToCompareTo = default);
        void Hide();
        BuildingVariantDetailController GetBuildingVariantDetailController();
        UnitVariantDetailController GetUnitVariantDetailController();
    }
}

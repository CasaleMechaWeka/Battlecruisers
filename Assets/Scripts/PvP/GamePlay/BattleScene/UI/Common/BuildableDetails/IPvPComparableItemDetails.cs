using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.LoadoutScreen.Comparisons;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public interface IPvPComparableItemDetails<TItem> : IPvPDismissableEmitter where TItem : IPvPComparableItem
    {
        void ShowItemDetails(TItem item, TItem itemToCompareTo = default);
        void Hide();
    }
}

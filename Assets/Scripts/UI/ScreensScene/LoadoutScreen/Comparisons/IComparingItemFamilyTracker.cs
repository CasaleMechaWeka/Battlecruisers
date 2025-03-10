using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons
{
    public interface IComparingItemFamilyTracker
    {
        IBroadcastingProperty<ItemFamily?> ComparingFamily { get; }

        void SetComparingFamily(ItemFamily? comparingItemFamily);
    }
}
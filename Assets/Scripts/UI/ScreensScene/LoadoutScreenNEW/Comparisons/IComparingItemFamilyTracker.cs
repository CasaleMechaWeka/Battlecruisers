using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons
{
    public interface IComparingItemFamilyTracker
    {
        IBroadcastingProperty<ItemFamily?> ComparingFamily { get; }

        void SetComparingFamily(ItemFamily? comparingItemFamily);
    }
}
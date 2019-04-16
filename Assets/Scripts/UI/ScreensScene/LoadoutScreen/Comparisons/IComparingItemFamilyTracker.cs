using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using UnityCommon.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons
{
    public interface IComparingItemFamilyTracker
    {
        IBroadcastingProperty<ItemFamily?> ComparingFamily { get; }

        void SetComparingFamily(ItemFamily? comparingItemFamily);
    }
}
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using UnityCommon.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons
{
    public class ComparingItemFamilyTracker : IComparingItemFamilyTracker
    {
        private ISettableBroadcastingProperty<ItemFamily?> _comparingFamily;
        public IBroadcastingProperty<ItemFamily?> ComparingFamily { get; }

        public ComparingItemFamilyTracker()
        {
            _comparingFamily = new SettableBroadcastingProperty<ItemFamily?>(initialValue: null);
            ComparingFamily = new BroadcastingProperty<ItemFamily?>(_comparingFamily);
        }

        public void SetComparingFamily(ItemFamily? comparingItemFamily)
        {
            _comparingFamily.Value = comparingItemFamily;
        }
    }
}
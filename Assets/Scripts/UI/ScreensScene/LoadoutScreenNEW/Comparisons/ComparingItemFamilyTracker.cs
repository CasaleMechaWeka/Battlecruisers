using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons
{
    public class ComparingItemFamilyTracker : IComparingItemFamilyTracker
    {
        private ISettableBroadcastingProperty<ItemFamily?> _comparingFamily;
        public IBroadcastingProperty<ItemFamily?> ComparingFamily { get; private set; }

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
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates
{
    public class LoadoutItemDefaultState<TItem> : DefaultState<TItem> where TItem : class, IComparableItem
    {
        private readonly IItemDetailsManager<TItem> _itemDetailsManager;

        public LoadoutItemDefaultState(IItemDetailsManager<TItem> itemDetailsManager, IItem<TItem> item)
            : base(item)
        {
            Assert.IsNotNull(itemDetailsManager);
            _itemDetailsManager = itemDetailsManager;
        }

        public override void SelectItem()
        {
            _itemDetailsManager.SelectItem(_item);
        }
    }
}

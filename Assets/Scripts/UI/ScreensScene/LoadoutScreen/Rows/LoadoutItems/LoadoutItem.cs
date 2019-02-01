using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public abstract class LoadoutItem<TItem> : BaseItem<TItem> 
        where TItem : class, IComparableItem
    {
        public override void Initialise(TItem item, IItemDetailsManager<TItem> itemDetailsManager) 
        {
            base.Initialise(item, itemDetailsManager);
            GoToState(UIState.Default);
        }

        protected override IItemState<TItem> CreateDefaultState()
        {
            return new LoadoutItemDefaultState<TItem>(_itemDetailsManager, this);
        }
    }
}

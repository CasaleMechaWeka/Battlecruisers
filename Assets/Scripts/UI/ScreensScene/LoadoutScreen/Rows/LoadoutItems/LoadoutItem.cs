using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public abstract class LoadoutItem<TItem> : BaseItem<TItem> where TItem : IComparableItem
    {
        public override void Initialise(TItem item, IItemDetailsManager<TItem> itemDetailsManager) 
        {
            base.Initialise(item, itemDetailsManager);
                
            GoToDefaultState();
        }

        public sealed override void GoToDefaultState()
        {
            _state = new LoadoutItemDefaultState<TItem>(_itemDetailsManager, this);
        }
    }
}

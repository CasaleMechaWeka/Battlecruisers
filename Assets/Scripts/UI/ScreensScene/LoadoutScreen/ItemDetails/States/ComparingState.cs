using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails.States
{
    public class ComparingState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
        public ComparingState(IItemDetailsManager<TItem> itemDetailsManager, IItemStateManager itemStateManager)
            : base(itemDetailsManager, itemStateManager) 
        {
            _itemStateManager.HandleDetailsManagerComparing();
        }
	}
}

using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates
{
    public class DisabledState<TItem> : ItemState<TItem> where TItem : class, IComparableItem
    {
        protected override Color BackgroundColour { get { return BaseItem<TItem>.Colors.DEFAULT; } }

        public DisabledState(IItem<TItem> item)
            : base(item)
        {
        }

        public override void SelectItem()
        {
            // Disabled, so do nothing :)
        }
    }
}

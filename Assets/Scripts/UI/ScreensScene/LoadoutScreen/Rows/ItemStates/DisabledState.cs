using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.ItemStates
{
    public class DisabledState<TItem> : ItemState<TItem> where TItem : IComparableItem
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

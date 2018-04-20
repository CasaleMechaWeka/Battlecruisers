using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems.States
{
    public class DisabledState<TItem> : UnlockedItemState<TItem> where TItem : IComparableItem
    {
        protected override Color BackgroundColour { get { return BaseItem<TItem>.Colors.DEFAULT; } }

        public DisabledState(UnlockedItem<TItem> item)
            : base(item)
        {
        }

        public override void SelectItem()
        {
            // Disabled, so do nothing :)
        }
    }
}

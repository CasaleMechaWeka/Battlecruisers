using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
	public class UnlockedHullItem : UnlockedItem<ICruiser>
	{
        public override ItemType Type { get { return ItemType.Cruiser; } }

        public void OnNewHullSelected(ICruiser selectedCruiser)
		{
			IsItemInLoadout = ReferenceEquals(selectedCruiser, Item);
		}
	}
}

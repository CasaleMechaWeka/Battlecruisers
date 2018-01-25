using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
	public class UnlockedHullItem : UnlockedItem<ICruiser>
	{
		public void OnNewHullSelected(ICruiser selectedCruiser)
		{
			IsItemInLoadout = ReferenceEquals(selectedCruiser, Item);
		}
	}
}

using BattleCruisers.Cruisers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	public class UnlockedHullItem : UnlockedItem<ICruiser>
	{
		public void OnNewHullSelected(ICruiser selectedCruiser)
		{
			IsItemInLoadout = object.ReferenceEquals(selectedCruiser, Item);
		}
	}
}

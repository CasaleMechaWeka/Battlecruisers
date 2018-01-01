using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
	// Need this class, because Unity does not allow generic scripts to be referenced in components
	public class UnlockedBuildingItem : UnlockedItem<IBuilding> { }
}

using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems
{
	// Need this class, because Unity does not allow generic scripts to be referenced in components
	public class UnlockedBuildingItem : UnlockedItem<IBuilding> { }
}

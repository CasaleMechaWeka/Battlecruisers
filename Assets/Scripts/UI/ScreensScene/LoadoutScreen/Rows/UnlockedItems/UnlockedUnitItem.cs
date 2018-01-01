using BattleCruisers.Buildables.Units;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems
{
    // Need this class, because Unity does not allow generic scripts to be referenced in components
    public class UnlockedUnitItem : UnlockedItem<IUnit> { }
}

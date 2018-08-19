using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class UnitSection : BuildableSection<IUnit, UnitKey>
    {
        protected override ItemType ItemType { get { return ItemType.Unit; } }
    }
}
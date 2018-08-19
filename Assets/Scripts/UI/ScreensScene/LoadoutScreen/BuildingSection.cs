using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class BuildingSection : BuildableSection<IBuilding, BuildingKey>
    {
        protected override ItemType ItemType { get { return ItemType.Building; } }
    }
}
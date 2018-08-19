using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutBuildingItemWrapper : LoadoutItemWrapper<IBuilding, BuildingKey>
    {
        protected override bool IsItemUnlocked()
        {
            return _gameModel.UnlockedBuildings.Contains(_itemKey);
        }
    }
}
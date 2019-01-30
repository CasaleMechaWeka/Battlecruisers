using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class BuildingItemContainer : ItemContainer
    {
        public BuildingKey buildingKey;

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            return gameModel.UnlockedBuildings.Contains(buildingKey);
        }
    }
}
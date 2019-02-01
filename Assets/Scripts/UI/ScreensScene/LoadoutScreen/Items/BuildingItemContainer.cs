using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public class BuildingItemContainer : ItemContainer
    {
        public PrefabKeyName buildingKeyName;

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            BuildingKey buildingKey = StaticPrefabKeyHelper.GetPrefabKey<BuildingKey>(buildingKeyName);
            return gameModel.UnlockedBuildings.Contains(buildingKey);
        }
    }
}
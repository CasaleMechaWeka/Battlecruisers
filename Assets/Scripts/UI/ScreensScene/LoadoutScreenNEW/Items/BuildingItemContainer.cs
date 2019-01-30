using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
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
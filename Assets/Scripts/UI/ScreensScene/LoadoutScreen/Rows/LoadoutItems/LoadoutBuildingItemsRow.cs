using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutBuildingItemsRow : LoadoutBuildableItemsRow<IBuilding, BuildingKey>
    {
        private BuildingCategory _buildingCategory;

        protected override int NumOfLockedBuildables { get { return _lockedInfo.NumOfLockedBuildings(_buildingCategory); } }

        public void Initialise(IItemsRowArgs<IBuilding> args, BuildingCategory buildingCategory)
        {
            base.Initialise(args);

            _buildingCategory = buildingCategory;
        }

        protected override LoadoutItem<IBuilding> CreateItem(IBuilding item)
        {
            return _uiFactory.CreateLoadoutBuildingItem(_layoutGroup, item);
        }

        protected override IBuilding GetBuildablePrefab(BuildingKey prefabKey)
        {
            return _prefabFactory.GetBuildingWrapperPrefab(prefabKey).Buildable;
        }

        protected override IList<IBuilding> GetLoadoutBuildablePrefabs()
        {
            return GetBuildablePrefabs(_gameModel.PlayerLoadout.GetBuildings(_buildingCategory));
        }
    }
}

using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems
{
    public class LoadoutBuildingItemsRow : LoadoutBuildableItemsRow<IBuilding, BuildingKey>
    {
        private BuildingCategory _buildingCategory;

        public void Initialise(IItemsRowArgs<IBuilding> args, BuildingCategory buildingCategory)
        {
            // Will be used in overridden method called by base.Initialise() :/
            _buildingCategory = buildingCategory;

            base.Initialise(args);
        }

        protected override IList<BuildingKey> FindBuildableKeys(IStaticData staticData)
        {
            return
                staticData.BuildingKeys
                    .Where(key => key.BuildingCategory == _buildingCategory)
                    .ToList();
        }

        protected override IBuilding GetBuildablePrefab(IPrefabFactory prefabFactory, BuildingKey buildableKey)
        {
            return prefabFactory.GetBuildingWrapperPrefab(buildableKey).Buildable;
        }
    }
}

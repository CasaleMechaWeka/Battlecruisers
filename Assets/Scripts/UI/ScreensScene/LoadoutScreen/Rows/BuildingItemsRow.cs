using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public class BuildingItemsRow : BuildableItemsRow<IBuilding, BuildingKey>
    {
        private readonly BuildingCategory _buildingCategory;

        public BuildingItemsRow(
            IGameModel gameModel, 
            IPrefabFactory prefabFactory, 
            IUIFactory uiFactory, 
            LoadoutBuildableItemsRow<IBuilding> loadoutRow, 
            UnlockedBuildableItemsRow<IBuilding> unlockedRow, 
            IItemDetailsManager<IBuilding> detailsManager,
            BuildingCategory buildingCategory) 
            : base(gameModel, prefabFactory, uiFactory, loadoutRow, unlockedRow, detailsManager)
        {
            _buildingCategory = buildingCategory;
        }

        protected override IList<IBuilding> GetLoadoutBuildablePrefabs()
        {
            return GetBuildablePrefabs(_gameModel.PlayerLoadout.GetBuildings(_buildingCategory), addToDictionary: false);
        }

        protected override IList<IBuilding> GetUnlockedBuildingPrefabs()
        {
            return GetBuildablePrefabs(_gameModel.GetUnlockedBuildings(_buildingCategory), addToDictionary: true);
        }

		protected override IBuilding GetBuildablePrefab(BuildingKey prefabKey)
		{
            return _prefabFactory.GetBuildingWrapperPrefab(prefabKey).Buildable;
		}
		
        protected override void AddToLoadoutModel(BuildingKey buildableKey)
        {
            _gameModel.PlayerLoadout.AddBuilding(buildableKey);
        }

        protected override void RemoveFromLoadoutModel(BuildingKey buildableKey)
        {
            _gameModel.PlayerLoadout.RemoveBuilding(buildableKey);
        }
    }
}

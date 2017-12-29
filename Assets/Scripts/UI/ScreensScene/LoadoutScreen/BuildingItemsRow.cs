using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class BuildingItemsRow : ItemsRow<IBuilding>
	{
		private readonly BuildingCategory _buildingCategory;
		private readonly LoadoutBuildingItemsRow _loadoutRow;
		private readonly UnlockedBuildingItemsRow _unlockedRow;
		private readonly IDictionary<IBuilding, BuildingKey> _buildingToKey;

		public BuildingItemsRow(
            IGameModel gameModel, 
            IPrefabFactory prefabFactory, 
            IUIFactory uiFactory, 
            BuildingCategory buildingCategory, 
			LoadoutBuildingItemsRow loadoutRow, 
            UnlockedBuildingItemsRow unlockedRow, 
            BuildingDetailsManager detailsManager)
			: base(gameModel, prefabFactory)
		{
			_buildingCategory = buildingCategory;
			_loadoutRow = loadoutRow;
			_unlockedRow = unlockedRow;

			_buildingToKey = new Dictionary<IBuilding, BuildingKey>();

			IList<IBuilding> loadoutBuildings = GetLoadoutBuildingPrefabs(_buildingCategory);
			_loadoutRow.Initialise(uiFactory, loadoutBuildings, detailsManager);
			IList<IBuilding> unlockedBuildings = GetUnlockedBuildingPrefabs(_buildingCategory);
			_unlockedRow.Initialise(this, uiFactory, unlockedBuildings, loadoutBuildings, detailsManager);
		}

		private IList<IBuilding> GetLoadoutBuildingPrefabs(BuildingCategory buildingCategory)
		{
			return GetBuildingPrefabs(_gameModel.PlayerLoadout.GetBuildings(buildingCategory), addToDictionary: false);
		}

		private IList<IBuilding> GetUnlockedBuildingPrefabs(BuildingCategory buildingCategory)
		{
			return GetBuildingPrefabs(_gameModel.GetUnlockedBuildings(buildingCategory), addToDictionary: true);
		}

		private IList<IBuilding> GetBuildingPrefabs(IList<BuildingKey> buildingKeys, bool addToDictionary)
		{
			IList<IBuilding> prefabs = new List<IBuilding>();

			foreach (BuildingKey key in buildingKeys)
			{
                IBuilding building = _prefabFactory.GetBuildingWrapperPrefab(key).Buildable;
				prefabs.Add(building);

				if (addToDictionary)
				{
					_buildingToKey.Add(building, key);
				}
			}

			return prefabs;
		}

		public override bool SelectUnlockedItem(UnlockedItem<IBuilding> buildableItem)
		{
			bool isItemInLoadout = false;

			if (buildableItem.IsItemInLoadout)
			{
				RemoveBuildingFromLoadout(buildableItem.Item);
			}
			else if (CanAddBuilding())
			{
				AddBuildingToLoadout(buildableItem.Item);
				isItemInLoadout = true;
			}

			return isItemInLoadout;
		}

		private bool CanAddBuilding()
		{
			return _loadoutRow.CanAddBuilding();
		}

		private void AddBuildingToLoadout(IBuilding building)
		{
			_gameModel.PlayerLoadout.AddBuilding(_buildingToKey[building]);
			_loadoutRow.AddBuilding(building);
		}

		private void RemoveBuildingFromLoadout(IBuilding building)
		{
			_gameModel.PlayerLoadout.RemoveBuilding(_buildingToKey[building]);
			_loadoutRow.RemoveBuilding(building);
		}
	}
}

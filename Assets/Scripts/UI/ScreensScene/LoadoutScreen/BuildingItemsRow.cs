using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class BuildingItemsRow : ItemsRow<Building>
	{
		private readonly BuildingCategory _buildingCategory;
		private readonly LoadoutBuildingItemsRow _loadoutRow;
		private readonly UnlockedBuildingItemsRow _unlockedRow;
		private readonly IDictionary<Building, BuildingKey> _buildingToKey;

		public BuildingItemsRow(IGameModel gameModel, IPrefabFactory prefabFactory, IUIFactory uiFactory, BuildingCategory buildingCategory, 
			LoadoutBuildingItemsRow loadoutRow, UnlockedBuildingItemsRow unlockedRow, BuildingDetailsManager detailsManager)
			: base(gameModel, prefabFactory)
		{
			_buildingCategory = buildingCategory;
			_loadoutRow = loadoutRow;
			_unlockedRow = unlockedRow;

			_buildingToKey = new Dictionary<Building, BuildingKey>();

			IList<Building> loadoutBuildings = GetLoadoutBuildingPrefabs(_buildingCategory);
			_loadoutRow.Initialise(uiFactory, loadoutBuildings, detailsManager);
			IList<Building> unlockedBuildings = GetUnlockedBuildingPrefabs(_buildingCategory);
			_unlockedRow.Initialise(this, uiFactory, unlockedBuildings, loadoutBuildings, detailsManager);
		}

		private IList<Building> GetLoadoutBuildingPrefabs(BuildingCategory buildingCategory)
		{
			return GetBuildingPrefabs(_gameModel.PlayerLoadout.GetBuildings(buildingCategory), addToDictionary: false);
		}

		private IList<Building> GetUnlockedBuildingPrefabs(BuildingCategory buildingCategory)
		{
			return GetBuildingPrefabs(_gameModel.GetUnlockedBuildings(buildingCategory), addToDictionary: true);
		}

		private IList<Building> GetBuildingPrefabs(IList<BuildingKey> buildingKeys, bool addToDictionary)
		{
			IList<Building> prefabs = new List<Building>();

			foreach (BuildingKey key in buildingKeys)
			{
				Building building = _prefabFactory.GetBuildingWrapperPrefab(key).Buildable;
				prefabs.Add(building);

				if (addToDictionary)
				{
					_buildingToKey.Add(building, key);
				}
			}

			return prefabs;
		}

		public override bool SelectUnlockedItem(UnlockedItem<Building> buildableItem)
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

		private void AddBuildingToLoadout(Building building)
		{
			_gameModel.PlayerLoadout.AddBuilding(_buildingToKey[building]);
			_loadoutRow.AddBuilding(building);
		}

		private void RemoveBuildingFromLoadout(Building building)
		{
			_gameModel.PlayerLoadout.RemoveBuilding(_buildingToKey[building]);
			_loadoutRow.RemoveBuilding(building);
		}
	}
}

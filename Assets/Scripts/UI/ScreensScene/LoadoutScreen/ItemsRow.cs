using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using System;
using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class ItemsRow
	{
		private readonly IGameModel _gameModel;
		private readonly IPrefabFactory _prefabFactory;
		private readonly BuildingCategory _buildingCategory;
		private readonly LoadoutItemsRow _loadoutRow;
		private readonly UnlockedBuildableItemsRow _unlockedRow;
		private readonly IDictionary<Building, BuildingKey> _buildingToKey;

		public ItemsRow(IGameModel gameModel, IPrefabFactory prefabFactory, IUIFactory uiFactory, 
			BuildingCategory buildingCategory, LoadoutItemsRow loadoutRow, UnlockedBuildableItemsRow unlockedRow)
		{
			_gameModel = gameModel;
			_prefabFactory = prefabFactory;
			_buildingCategory = buildingCategory;
			_loadoutRow = loadoutRow;
			_unlockedRow = unlockedRow;

			_buildingToKey = new Dictionary<Building, BuildingKey>();

			IList<Building> loadoutBuildings = GetLoadoutBuildingPrefabs(_buildingCategory);
			_loadoutRow.Initialise(uiFactory, loadoutBuildings);
			IList<Building> unlockedBuildings = GetUnlockedBuildingPrefabs(_buildingCategory);
			_unlockedRow.Initialise(this, uiFactory, unlockedBuildings, loadoutBuildings);
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
				Building building = _prefabFactory.GetBuildingWrapperPrefab(key).Building;
				prefabs.Add(building);

				if (addToDictionary)
				{
					_buildingToKey.Add(building, key);
				}
			}

			return prefabs;
		}

		public bool CanAddBuilding()
		{
			return _loadoutRow.CanAddBuilding();
		}

		public void AddBuildingToLoadout(Building building)
		{
			_gameModel.PlayerLoadout.AddBuilding(_buildingToKey[building]);
			_loadoutRow.AddBuilding(building);
		}

		public void RemoveBuildingFromLoadout(Building building)
		{
			_gameModel.PlayerLoadout.RemoveBuilding(_buildingToKey[building]);
			_loadoutRow.RemoveBuilding(building);
		}
	}
}

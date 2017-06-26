using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using System;
using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class BuildableItemsRow : ItemsRow<Building>
	{
		private readonly BuildingCategory _buildingCategory;
		private readonly LoadoutBuildableItemsRow _loadoutRow;
		private readonly UnlockedBuildableItemsRow _unlockedRow;
		private readonly IDictionary<Building, BuildingKey> _buildingToKey;

		public BuildableItemsRow(IGameModel gameModel, IPrefabFactory prefabFactory, IUIFactory uiFactory, BuildingCategory buildingCategory, 
			LoadoutBuildableItemsRow loadoutRow, UnlockedBuildableItemsRow unlockedRow, BuildableDetailsManager detailsManager)
			: base(gameModel, prefabFactory)
		{
			_buildingCategory = buildingCategory;
			_loadoutRow = loadoutRow;
			_unlockedRow = unlockedRow;

			_buildingToKey = new Dictionary<Building, BuildingKey>();

			IList<Building> loadoutBuildings = GetLoadoutBuildingPrefabs(_buildingCategory);
			_loadoutRow.Initialise(uiFactory, loadoutBuildings);
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
				Building building = _prefabFactory.GetBuildingWrapperPrefab(key).Building;
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
			else
			{
				// FELIX  Show error to user?  BETTER => disable all buttons that would add an item :D
				// => Create Unaddable UnlockedItem state :)
				throw new NotImplementedException();
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

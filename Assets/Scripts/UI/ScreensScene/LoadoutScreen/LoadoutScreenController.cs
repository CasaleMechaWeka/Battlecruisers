using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	// FELIX  Avoid potential duplicate code with BattleSceneGod?
	public class LoadoutScreenController : ScreenController
	{
		private IDataProvider _dataProvider;
		private IGameModel _gameModel;
		private IPrefabFactory _prefabFactory;
		private IUIFactory _uiFactory;
		private IDictionary<BuildingCategory, LoadoutItemsRow> _buildingCategoryToLoadoutRow;
		private IDictionary<Building, BuildingKey> _buildingToKey;

		// FELIX  Avoid duplication between Loadout items and Unlocked items :)
		public LoadoutItemsRow factoriesRow, defensivesRow, offensivesRow, tacticalsRow;
		public UnlockedItemsRow unlockedFactoriesRow, unlockedDefensivesRow, unlockedOffensivesRow, unlockedTacticalsRow;

		public void Initialise(IScreensSceneGod screensSceneGod, IDataProvider dataProvider, IPrefabFactory prefabFactory, IUIFactory uiFactory)
		{
			base.Initialise(screensSceneGod);

			Assert.IsNotNull(dataProvider);
			Assert.IsNotNull(prefabFactory);
			Assert.IsNotNull(uiFactory);

			_dataProvider = dataProvider;
			_gameModel = _dataProvider.GameModel;
			_prefabFactory = prefabFactory;
			_uiFactory = uiFactory;

			_buildingCategoryToLoadoutRow = new Dictionary<BuildingCategory, LoadoutItemsRow>();
			_buildingToKey = new Dictionary<Building, BuildingKey>();

			_buildingCategoryToLoadoutRow.Add(BuildingCategory.Factory, factoriesRow);
			_buildingCategoryToLoadoutRow.Add(BuildingCategory.Defence, defensivesRow);
			_buildingCategoryToLoadoutRow.Add(BuildingCategory.Offence, offensivesRow);
			_buildingCategoryToLoadoutRow.Add(BuildingCategory.Tactical, tacticalsRow);		

			ShowItems();
		}

		private void ShowItems()
		{
			// FELIX  Do hull row last, as different to building rows
//			Cruiser playerCruiserPrefab = _prefabFactory.GetCruiserPrefab(_gameModel.PlayerLoadout.Hull);

			// FELIX  There has to be a better way :P  LoadoutScreenRow?
//			foreach (KeyValuePair<BuildingCategory, LoadoutItemsRow> pair in _buildingCategoryToLoadoutRow)
//			{
//				IList<Building> loadoutBuildings = GetLoadoutBuildingPrefabs(pair.Key);
//				
//			}

			IList<Building> loadoutFactories = GetLoadoutBuildingPrefabs(BuildingCategory.Factory);
			factoriesRow.Initialise(_uiFactory, loadoutFactories);
			IList<Building> unlockedFactories = GetLoadoutBuildingPrefabs(BuildingCategory.Factory);
			unlockedFactoriesRow.Initialise(this, _uiFactory, unlockedFactories, loadoutFactories);

			IList<Building> loadoutDefensives = GetLoadoutBuildingPrefabs(BuildingCategory.Defence);
			defensivesRow.Initialise(_uiFactory, loadoutDefensives);
			IList<Building> unlockedDefensives = GetUnlockedBuildingPrefabs(BuildingCategory.Defence);
			unlockedDefensivesRow.Initialise(this, _uiFactory, unlockedDefensives, loadoutDefensives);

			IList<Building> loadoutOffensives = GetLoadoutBuildingPrefabs(BuildingCategory.Offence);
			offensivesRow.Initialise(_uiFactory, loadoutOffensives);
			IList<Building> unlockedOffensives = GetUnlockedBuildingPrefabs(BuildingCategory.Offence);
			unlockedOffensivesRow.Initialise(this, _uiFactory, unlockedOffensives, loadoutOffensives);

			IList<Building> loadoutTacticals = GetLoadoutBuildingPrefabs(BuildingCategory.Tactical);
			tacticalsRow.Initialise(_uiFactory, loadoutTacticals);
			IList<Building> unlockedTacticals = GetUnlockedBuildingPrefabs(BuildingCategory.Tactical);
			unlockedTacticalsRow.Initialise(this, _uiFactory, unlockedTacticals, loadoutTacticals);

			// FELIX  Ultras?
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

		public bool CanAddBuilding(BuildingCategory buildingCategory)
		{
			return _buildingCategoryToLoadoutRow[buildingCategory].CanAddBuilding();
		}

		public void AddBuildingToLoadout(Building building)
		{
			_gameModel.PlayerLoadout.AddBuilding(_buildingToKey[building]);

			LoadoutItemsRow loadoutRow = _buildingCategoryToLoadoutRow[building.category];
			loadoutRow.AddBuilding(building);
		}

		public void RemoveBuildingFromLoadout(Building building)
		{
			_gameModel.PlayerLoadout.RemoveBuilding(_buildingToKey[building]);

			LoadoutItemsRow loadoutRow = _buildingCategoryToLoadoutRow[building.category];
			loadoutRow.RemoveBuilding(building);
		}

		public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}

		public void Save()
		{
			_dataProvider.SaveGame();
		}
	}
}

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
		private IGameModel _gameModel;
		private IPrefabFactory _prefabFactory;
		private IUIFactory _uiFactory;

		// FELIX  Avoid duplication between Loadout items and Unlocked items :)
		public LoadoutItemsRow factoriesRow, defensivesRow, offensivesRow, tacticalsRow;
		public LoadoutItemsRow unlockedFactoriesRow, unlockedDefensivesRow;

		public void Initialise(IScreensSceneGod screensSceneGod, IGameModel gameModel, IPrefabFactory prefabFactory, IUIFactory uiFactory)
		{
			base.Initialise(screensSceneGod);

			Assert.IsNotNull(gameModel);
			Assert.IsNotNull(prefabFactory);
			Assert.IsNotNull(uiFactory);

			_gameModel = gameModel;
			_prefabFactory = prefabFactory;
			_uiFactory = uiFactory;

			ShowLoadout();
			ShowUnlockedItems();
		}

		private void ShowLoadout()
		{
			// FELIX  Do hull row last, as different to building rows
//			Cruiser playerCruiserPrefab = _prefabFactory.GetCruiserPrefab(_gameModel.PlayerLoadout.Hull);

			IList<Building> factories = GetLoadoutBuildingPrefabs(BuildingCategory.Factory);
			factoriesRow.Initialise(_uiFactory, factories);

			IList<Building> defensives = GetLoadoutBuildingPrefabs(BuildingCategory.Defence);
			defensivesRow.Initialise(_uiFactory, defensives);

			IList<Building> offensives = GetLoadoutBuildingPrefabs(BuildingCategory.Offence);
			offensivesRow.Initialise(_uiFactory, offensives);

			IList<Building> tacticals = GetLoadoutBuildingPrefabs(BuildingCategory.Tactical);
			tacticalsRow.Initialise(_uiFactory, tacticals);

			// FELIX  Ultras?
		}

		private void ShowUnlockedItems()
		{
			IList<Building> factories = GetUnlockedBuildingPrefabs(BuildingCategory.Factory);
			unlockedFactoriesRow.Initialise(_uiFactory, factories);

			IList<Building> defensives = GetUnlockedBuildingPrefabs(BuildingCategory.Defence);
			unlockedDefensivesRow.Initialise(_uiFactory, defensives);
			
			// FELIX  NEXT  Ohter building types :)
		}

		private IList<Building> GetLoadoutBuildingPrefabs(BuildingCategory buildingCategory)
		{
			return GetBuildingPrefabs(_gameModel.GetUnlockedBuildings(buildingCategory));
		}

		// FELIX  Exclude buildings already in loadout
		private IList<Building> GetUnlockedBuildingPrefabs(BuildingCategory buildingCategory)
		{
			// FELIX TEMP  Want to practise with scroll bars :)
			IList<Building> buildings = GetBuildingPrefabs(_gameModel.PlayerLoadout.GetBuildings(buildingCategory));
			IList<Building> duplicates = new List<Building>();
			foreach (Building building in buildings)
			{
				duplicates.Add(building);
				duplicates.Add(building);
			}
			return duplicates;
//			return GetBuildingPrefabs(_gameModel.PlayerLoadout.GetBuildings(buildingCategory));
		}

		private IList<Building> GetBuildingPrefabs(IList<BuildingKey> buildingKeys)
		{
			IList<Building> prefabs = new List<Building>();
			
			foreach (BuildingKey key in buildingKeys)
			{
				prefabs.Add(_prefabFactory.GetBuildingWrapperPrefab(key).Building);
			}

			return prefabs;
		}

		public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}

		public void GoToLevelsScreen()
		{
			_screensSceneGod.GoToLevelsScreen();
		}
	}
}

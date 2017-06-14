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
		public UnlockedItemsRow unlockedFactoriesRow, unlockedDefensivesRow;

		public void Initialise(IScreensSceneGod screensSceneGod, IGameModel gameModel, IPrefabFactory prefabFactory, IUIFactory uiFactory)
		{
			base.Initialise(screensSceneGod);

			Assert.IsNotNull(gameModel);
			Assert.IsNotNull(prefabFactory);
			Assert.IsNotNull(uiFactory);

			_gameModel = gameModel;
			_prefabFactory = prefabFactory;
			_uiFactory = uiFactory;

			ShowItems();
		}

		private void ShowItems()
		{
			// FELIX  Do hull row last, as different to building rows
//			Cruiser playerCruiserPrefab = _prefabFactory.GetCruiserPrefab(_gameModel.PlayerLoadout.Hull);

			IList<Building> loadoutFactories = GetLoadoutBuildingPrefabs(BuildingCategory.Factory);
			factoriesRow.Initialise(_uiFactory, loadoutFactories);

			IList<Building> loadoutDefensives = GetLoadoutBuildingPrefabs(BuildingCategory.Defence);
			defensivesRow.Initialise(_uiFactory, loadoutDefensives);
			IList<Building> unlockedDefensives = GetUnlockedBuildingPrefabs(BuildingCategory.Defence);
			unlockedDefensivesRow.Initialise(_uiFactory, unlockedDefensives, loadoutDefensives);

			IList<Building> loadoutOffensives = GetLoadoutBuildingPrefabs(BuildingCategory.Offence);
			offensivesRow.Initialise(_uiFactory, loadoutOffensives);

			IList<Building> loadoutTacticals = GetLoadoutBuildingPrefabs(BuildingCategory.Tactical);
			tacticalsRow.Initialise(_uiFactory, loadoutTacticals);

			// FELIX  Ultras?
		}

		private IList<Building> GetLoadoutBuildingPrefabs(BuildingCategory buildingCategory)
		{
			return GetBuildingPrefabs(_gameModel.PlayerLoadout.GetBuildings(buildingCategory));
		}

		// FELIX  Give feedback for buildings already in loadout
		private IList<Building> GetUnlockedBuildingPrefabs(BuildingCategory buildingCategory)
		{
			return GetBuildingPrefabs(_gameModel.GetUnlockedBuildings(buildingCategory));
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

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

		public LoadoutItemsRow factoriesRow;

		public new void Initialise(IScreensSceneGod screensSceneGod, IGameModel gameModel, IPrefabFactory prefabFactory, IUIFactory uiFactory)
		{
			base.Initialise(screensSceneGod);

			Assert.IsNotNull(gameModel);
			Assert.IsNotNull(prefabFactory);
			Assert.IsNotNull(uiFactory);

			_gameModel = gameModel;
			_prefabFactory = prefabFactory;
			_uiFactory = uiFactory;

			ShowLoadout();
		}

		private void ShowLoadout()
		{
			// FELIX  Do hull row last, as different to building rows
//			Cruiser playerCruiserPrefab = _prefabFactory.GetCruiserPrefab(_gameModel.PlayerLoadout.Hull);

			IList<Building> factories = GetBuildingPrefabs(BuildingCategory.Factory);
			factoriesRow.Initialise(_uiFactory, factories);
			// FELIX  Ultras?
		}

		private IList<Building> GetBuildingPrefabs(BuildingCategory buildingCategory)
		{
			IList<BuildingKey> keys = _gameModel.PlayerLoadout.GetBuildings(buildingCategory);
			IList<Building> prefabs = new List<Building>();
			
			foreach (BuildingKey key in keys)
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

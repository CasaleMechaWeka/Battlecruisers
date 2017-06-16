using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Hulls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public class LoadoutScreenController : ScreenController
	{
		private IDataProvider _dataProvider;
		private IGameModel _gameModel;
		private IPrefabFactory _prefabFactory;
		private IUIFactory _uiFactory;

		public LoadoutHullItem loadoutHullItem;
		public UnlockedHullsRow unlockedHullsRow;
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

			// Initialise hull row
			new HullsRow(_gameModel, _prefabFactory, _uiFactory, loadoutHullItem, unlockedHullsRow);

			// Initialise building rows
			new ItemsRow(_gameModel, _prefabFactory, _uiFactory, BuildingCategory.Factory, factoriesRow, unlockedFactoriesRow);
			new ItemsRow(_gameModel, _prefabFactory, _uiFactory, BuildingCategory.Defence, defensivesRow, unlockedDefensivesRow);
			new ItemsRow(_gameModel, _prefabFactory, _uiFactory, BuildingCategory.Offence, offensivesRow, unlockedOffensivesRow);
			new ItemsRow(_gameModel, _prefabFactory, _uiFactory, BuildingCategory.Tactical, tacticalsRow, unlockedTacticalsRow);
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

using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
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

		public UIFactory uiFactory;
		public LoadoutHullItem loadoutHullItem;
		public UnlockedHullsRow unlockedHullsRow;
		public LoadoutBuildableItemsRow factoriesRow, defensivesRow, offensivesRow, tacticalsRow;
		public UnlockedBuildableItemsRow unlockedFactoriesRow, unlockedDefensivesRow, unlockedOffensivesRow, unlockedTacticalsRow;
		public BuildableDetailsManager buildableDetailsManager;
		public CruiserDetailsManager cruiserDetailsManager;

		public void Initialise(IScreensSceneGod screensSceneGod, IDataProvider dataProvider, IPrefabFactory prefabFactory)
		{
			base.Initialise(screensSceneGod);

			Assert.IsNotNull(dataProvider);
			Assert.IsNotNull(prefabFactory);

			_dataProvider = dataProvider;
			_gameModel = _dataProvider.GameModel;
			_prefabFactory = prefabFactory;

			uiFactory.Initialise(buildableDetailsManager);

			// Initialise hull row
			new HullItemsRow(_gameModel, _prefabFactory, uiFactory, loadoutHullItem, unlockedHullsRow, cruiserDetailsManager);

			// Initialise building rows
			new BuildableItemsRow(_gameModel, _prefabFactory, uiFactory, BuildingCategory.Factory, factoriesRow, unlockedFactoriesRow);
			new BuildableItemsRow(_gameModel, _prefabFactory, uiFactory, BuildingCategory.Defence, defensivesRow, unlockedDefensivesRow);
			new BuildableItemsRow(_gameModel, _prefabFactory, uiFactory, BuildingCategory.Offence, offensivesRow, unlockedOffensivesRow);
			new BuildableItemsRow(_gameModel, _prefabFactory, uiFactory, BuildingCategory.Tactical, tacticalsRow, unlockedTacticalsRow);
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

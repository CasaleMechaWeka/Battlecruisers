using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class LoadoutScreenController : ScreenController
	{
		private IDataProvider _dataProvider;
		private IGameModel _gameModel;
		private IPrefabFactory _prefabFactory;

		public UIFactory uiFactory;
		public LoadoutHullItem loadoutHullItem;
		public UnlockedHullItemsRow unlockedHullsRow;
		public LoadoutBuildingItemsRow factoriesRow, defensivesRow, offensivesRow, tacticalsRow, ultrasRow;
		public UnlockedBuildingItemsRow unlockedFactoriesRow, unlockedDefensivesRow, unlockedOffensivesRow, unlockedTacticalsRow, unlockedUltrasRow;
		public BuildingDetailsManager buildingDetailsManager;
        public UnitDetailsManager unitDetailsManager;
		public CruiserDetailsManager cruiserDetailsManager;

		public void Initialise(IScreensSceneGod screensSceneGod, IDataProvider dataProvider, IPrefabFactory prefabFactory)
		{
			base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(
                uiFactory,
                loadoutHullItem,
                unlockedHullsRow,
                factoriesRow, defensivesRow, offensivesRow, tacticalsRow, ultrasRow,
                unlockedFactoriesRow, unlockedDefensivesRow, unlockedOffensivesRow, unlockedTacticalsRow, unlockedUltrasRow,
                buildingDetailsManager,
                unitDetailsManager,
                cruiserDetailsManager,
                dataProvider, 
                prefabFactory);

			_dataProvider = dataProvider;
			_gameModel = _dataProvider.GameModel;
			_prefabFactory = prefabFactory;

            buildingDetailsManager.Initialise();
            unitDetailsManager.Initialise();
            cruiserDetailsManager.Initialise();

            uiFactory.Initialise(buildingDetailsManager, unitDetailsManager);

            // Hulls row
            IItemsRow<ICruiser> hullsRow = new HullItemsRow(_gameModel, _prefabFactory, uiFactory, loadoutHullItem, unlockedHullsRow, cruiserDetailsManager);
            hullsRow.SetupUI();

			// Building rows
            IList<IItemsRow<IBuilding>> buildingsRows = new List<IItemsRow<IBuilding>>()
            {
				new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, factoriesRow, unlockedFactoriesRow, buildingDetailsManager, BuildingCategory.Factory),
				new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, defensivesRow, unlockedDefensivesRow, buildingDetailsManager, BuildingCategory.Defence),
				new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, offensivesRow, unlockedOffensivesRow, buildingDetailsManager, BuildingCategory.Offence),
				new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, tacticalsRow, unlockedTacticalsRow, buildingDetailsManager, BuildingCategory.Tactical),
				new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, ultrasRow, unlockedUltrasRow, buildingDetailsManager, BuildingCategory.Ultra)
            };
            foreach (IItemsRow<IBuilding> buildingsRow in buildingsRows)
            {
                buildingsRow.SetupUI();
            }
		}

		public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}

		public void Save()
		{
			_dataProvider.SaveGame();
            _screensSceneGod.GoToHomeScreen();
		}
	}
}

using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
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

        // Hulls
		public LoadoutHullItem loadoutHullItem;
		public UnlockedHullItemsRow unlockedHullsRow;
		public CruiserDetailsManager cruiserDetailsManager;

        // Buildings
        public LoadoutBuildingItemsRow factoriesRow, defensivesRow, offensivesRow, tacticalsRow, ultrasRow;
        public UnlockedBuildingItemsRow unlockedFactoriesRow, unlockedDefensivesRow, unlockedOffensivesRow, unlockedTacticalsRow, unlockedUltrasRow;
        public BuildingDetailsManager buildingDetailsManager;

        // Units
        public LoadoutUnitItemsRow shipsRow, aircraftRow;
        public UnlockedUnitItemsRow unlockedShipsRow, unlockedAircraftRow;
        public UnitDetailsManager unitDetailsManager;

		public void Initialise(IScreensSceneGod screensSceneGod, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(
                uiFactory,
                dataProvider,
                prefabFactory,
                // Hulls
                loadoutHullItem,
                unlockedHullsRow,
                cruiserDetailsManager,
                // Buildings
                factoriesRow, defensivesRow, offensivesRow, tacticalsRow, ultrasRow,
                unlockedFactoriesRow, unlockedDefensivesRow, unlockedOffensivesRow, unlockedTacticalsRow, unlockedUltrasRow,
                buildingDetailsManager,
                // Units
                shipsRow, aircraftRow,
                unlockedShipsRow, unlockedAircraftRow,
                unitDetailsManager);

            _dataProvider = dataProvider;
            _gameModel = _dataProvider.GameModel;
            _prefabFactory = prefabFactory;

            buildingDetailsManager.Initialise();
            unitDetailsManager.Initialise();
            cruiserDetailsManager.Initialise();

            uiFactory.Initialise(buildingDetailsManager, unitDetailsManager);

            SetupHullsRow();
            SetupBuildingRows();
            SetupUnitRows();
        }

        private void SetupHullsRow()
        {
            IItemsRow<ICruiser> hullsRow = new HullItemsRow(_gameModel, _prefabFactory, uiFactory, loadoutHullItem, unlockedHullsRow, cruiserDetailsManager);
            hullsRow.SetupUI();
        }

        private void SetupBuildingRows()
        {
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

        private void SetupUnitRows()
        {
            IList<IItemsRow<IUnit>> unitsRows = new List<IItemsRow<IUnit>>()
            {
                new UnitItemsRow(_gameModel, _prefabFactory, uiFactory, shipsRow, unlockedShipsRow, unitDetailsManager, UnitCategory.Naval),
                new UnitItemsRow(_gameModel, _prefabFactory, uiFactory, aircraftRow, unlockedAircraftRow, unitDetailsManager, UnitCategory.Aircraft)
            };
            foreach (IItemsRow<IUnit> unitsRow in unitsRows)
            {
                unitsRow.SetupUI();
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

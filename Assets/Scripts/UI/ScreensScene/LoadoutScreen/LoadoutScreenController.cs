using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.UnlockedItems;
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
            ItemsRowArgs<ICruiser> args = new ItemsRowArgs<ICruiser>(_gameModel, _prefabFactory, uiFactory, cruiserDetailsManager);
            IItemsRow<ICruiser> hullsRow = new HullItemsRow(args, loadoutHullItem, unlockedHullsRow);
            hullsRow.SetupUI();
        }

        private void SetupBuildingRows()
        {
            ItemsRowArgs<IBuilding> args = new ItemsRowArgs<IBuilding>(_gameModel, _prefabFactory, uiFactory, buildingDetailsManager);

            IList<IItemsRow<IBuilding>> buildingsRows = new List<IItemsRow<IBuilding>>()
            {
                new BuildingItemsRow(args, factoriesRow, unlockedFactoriesRow, BuildingCategory.Factory),
                new BuildingItemsRow(args, defensivesRow, unlockedDefensivesRow, BuildingCategory.Defence),
                new BuildingItemsRow(args, offensivesRow, unlockedOffensivesRow, BuildingCategory.Offence),
                new BuildingItemsRow(args, tacticalsRow, unlockedTacticalsRow, BuildingCategory.Tactical),
                new BuildingItemsRow(args, ultrasRow, unlockedUltrasRow, BuildingCategory.Ultra)
            };
            foreach (IItemsRow<IBuilding> buildingsRow in buildingsRows)
            {
                buildingsRow.SetupUI();
            }
        }

        private void SetupUnitRows()
        {
            ItemsRowArgs<IUnit> args = new ItemsRowArgs<IUnit>(_gameModel, _prefabFactory, uiFactory, unitDetailsManager);

            IList<IItemsRow<IUnit>> unitsRows = new List<IItemsRow<IUnit>>()
            {
                new UnitItemsRow(args, shipsRow, unlockedShipsRow, UnitCategory.Naval),
                new UnitItemsRow(args, aircraftRow, unlockedAircraftRow, UnitCategory.Aircraft)
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

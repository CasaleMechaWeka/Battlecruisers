using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class LoadoutScreenController : ScreenController
	{
		private IDataProvider _dataProvider;
		private IGameModel _gameModel;
		private IPrefabFactory _prefabFactory;

		public UIFactory uiFactory;
        public HullsRowWrapper hullsRow;
        public CruiserDetailsManager cruiserDetailsManager;
        public BuildingDetailsManager buildingDetailsManager;
        public UnitDetailsManager unitDetailsManager;

		public void Initialise(IScreensSceneGod screensSceneGod, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            base.Initialise(screensSceneGod);

            // General
            Helper.AssertIsNotNull(uiFactory, dataProvider, prefabFactory, hullsRow);
            // Item details managers
            Helper.AssertIsNotNull(cruiserDetailsManager, buildingDetailsManager, unitDetailsManager);

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
            hullsRow.Initialise(_gameModel, _prefabFactory, uiFactory, cruiserDetailsManager);
        }

        private void SetupBuildingRows()
        {
            ItemsRowArgs<IBuilding> args = new ItemsRowArgs<IBuilding>(_gameModel, _prefabFactory, uiFactory, buildingDetailsManager);

            BuildingsRowWrapper[] buildingRows = GetComponentsInChildren<BuildingsRowWrapper>();

            foreach (BuildingsRowWrapper buildingRow in buildingRows)
            {
                buildingRow.Initialise(args);
            }
        }

        private void SetupUnitRows()
        {
            ItemsRowArgs<IUnit> args = new ItemsRowArgs<IUnit>(_gameModel, _prefabFactory, uiFactory, unitDetailsManager);

            UnitsRowWrapper[] unitRows = GetComponentsInChildren<UnitsRowWrapper>();

            foreach (UnitsRowWrapper unitRow in unitRows)
            {
                unitRow.Initialise(args);
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

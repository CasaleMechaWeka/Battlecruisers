using System.Collections;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
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
        private IItemStateManager _itemStateManager;

		public UIFactory uiFactory;
        public HullsRowWrapper hullsRowWrapper;
        public CruiserDetailsManager cruiserDetailsManager;
        public BuildingDetailsManager buildingDetailsManager;
        public UnitDetailsManager unitDetailsManager;

        public IEnumerator Initialise(
            IScreensSceneGod screensSceneGod, 
            IDataProvider dataProvider, 
            IPrefabFactory prefabFactory, 
            ISpriteProvider spriteProvider)
        {
            base.Initialise(screensSceneGod);

            yield return null;

            // General
            Helper.AssertIsNotNull(uiFactory, dataProvider, prefabFactory, spriteProvider, hullsRowWrapper);
            // Item details managers
            Helper.AssertIsNotNull(cruiserDetailsManager, buildingDetailsManager, unitDetailsManager);

            _dataProvider = dataProvider;
            _gameModel = _dataProvider.GameModel;
            _prefabFactory = prefabFactory;
            _itemStateManager = new ItemStateManager();

            buildingDetailsManager.Initialise(spriteProvider, _itemStateManager);
            unitDetailsManager.Initialise(_itemStateManager);
            cruiserDetailsManager.Initialise(_itemStateManager);

            uiFactory.Initialise(buildingDetailsManager, unitDetailsManager, cruiserDetailsManager);

			yield return null;
            SetupHullsRow();

			yield return null;
            SetupBuildingRows();

			yield return null;
            SetupUnitRows();
        }

        private void SetupHullsRow()
        {
            IItemsRowArgs<ICruiser> args = new ItemsRowArgs<ICruiser>(_dataProvider, _prefabFactory, uiFactory, cruiserDetailsManager);
            hullsRowWrapper.Initialise(args);
            _itemStateManager.AddItem(hullsRowWrapper.HullsRow, ItemType.Cruiser);
        }

        private void SetupBuildingRows()
        {
            ItemsRowArgs<IBuilding> args = new ItemsRowArgs<IBuilding>(_dataProvider, _prefabFactory, uiFactory, buildingDetailsManager);

            BuildingsRowWrapper[] buildingRowWrappers = GetComponentsInChildren<BuildingsRowWrapper>();

            foreach (BuildingsRowWrapper buildingRowWrapper in buildingRowWrappers)
            {
                buildingRowWrapper.Initialise(args);
                _itemStateManager.AddItem(buildingRowWrapper.BuildablesRow, ItemType.Building);
            }
        }

        private void SetupUnitRows()
        {
            ItemsRowArgs<IUnit> args = new ItemsRowArgs<IUnit>(_dataProvider, _prefabFactory, uiFactory, unitDetailsManager);

            UnitsRowWrapper[] unitRowWrappers = GetComponentsInChildren<UnitsRowWrapper>();

            foreach (UnitsRowWrapper unitRowWrapper in unitRowWrappers)
            {
                unitRowWrapper.Initialise(args);
                _itemStateManager.AddItem(unitRowWrapper.BuildablesRow, ItemType.Unit);
            }
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);
            hullsRowWrapper.HullsRow.OnPresenting(activationParameter: null);
        }

        public void GoToHomeScreen()
		{
			_screensSceneGod.GoToHomeScreen();
		}

		public void Save()
		{
            if (!hullsRowWrapper.HullsRow.UserChosenHull.Equals(_gameModel.PlayerLoadout.Hull))
            {
                _gameModel.PlayerLoadout.Hull = hullsRowWrapper.HullsRow.UserChosenHull;
                _dataProvider.SaveGame();
            }

            _screensSceneGod.GoToHomeScreen();
		}
	}
}

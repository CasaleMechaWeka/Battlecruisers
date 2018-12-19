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
using System.Collections;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class LoadoutScreenController : ScreenController
	{
		private IDataProvider _dataProvider;
		private IGameModel _gameModel;
		private IPrefabFactory _prefabFactory;
        private IItemStateManager _itemStateManager;

        public HullsRowWrapper hullsRowWrapper;
        public CruiserDetailsManager cruiserDetailsManager;
        public BuildingDetailsManager buildingDetailsManager;
        public UnitDetailsManager unitDetailsManager;

        public IEnumerator Initialise(
            IScreensSceneGod screensSceneGod, 
            IDataProvider dataProvider, 
            IPrefabFactory prefabFactory)
        {
            base.Initialise(screensSceneGod);

            yield return null;

            // General
            Helper.AssertIsNotNull(dataProvider, prefabFactory, hullsRowWrapper);
            // Item details managers
            Helper.AssertIsNotNull(cruiserDetailsManager, buildingDetailsManager, unitDetailsManager);

            _dataProvider = dataProvider;
            _gameModel = _dataProvider.GameModel;
            _prefabFactory = prefabFactory;
            _itemStateManager = new ItemStateManager();

            buildingDetailsManager.Initialise(_itemStateManager);
            AddChildPresentable(buildingDetailsManager);

            unitDetailsManager.Initialise(_itemStateManager);
            AddChildPresentable(unitDetailsManager);

            // FELIX
            //cruiserDetailsManager.Initialise(_itemStateManager);
            //AddChildPresentable(cruiserDetailsManager);

            //yield return null;
            //         SetupHullsRow();

            //yield return null;
            //         SetupBuildingRows();

            //yield return null;
            //         SetupUnitRows();
        }

        private void SetupHullsRow()
        {
            IItemsRowArgs<ICruiser> args = new ItemsRowArgs<ICruiser>(_dataProvider, _prefabFactory, cruiserDetailsManager);
            hullsRowWrapper.Initialise(args);
            _itemStateManager.AddItem(hullsRowWrapper.HullsRow, ItemType.Cruiser);

            AddChildPresentable(hullsRowWrapper.HullsRow);
        }

        private void SetupBuildingRows()
        {
            ItemsRowArgs<IBuilding> args = new ItemsRowArgs<IBuilding>(_dataProvider, _prefabFactory, buildingDetailsManager);

            BuildingSection buildingSection = GetComponentInChildren<BuildingSection>();
            Assert.IsNotNull(buildingSection);
            buildingSection.Initialise(args, _itemStateManager);

            AddChildPresentable(buildingSection);
        }

        private void SetupUnitRows()
        {
            ItemsRowArgs<IUnit> args = new ItemsRowArgs<IUnit>(_dataProvider, _prefabFactory, unitDetailsManager);

            UnitSection unitSection = GetComponentInChildren<UnitSection>();
            Assert.IsNotNull(unitSection);
            unitSection.Initialise(args, _itemStateManager);

            AddChildPresentable(unitSection);
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

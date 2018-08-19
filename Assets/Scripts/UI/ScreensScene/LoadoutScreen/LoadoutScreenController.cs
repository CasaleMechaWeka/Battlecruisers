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
        private BuildingSection _buildingSection;
        private UnitSection _unitSection;

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

            _buildingSection = GetComponentInChildren<BuildingSection>();
            Assert.IsNotNull(_buildingSection);
            _buildingSection.Initialise(args, _itemStateManager);
        }

        private void SetupUnitRows()
        {
            ItemsRowArgs<IUnit> args = new ItemsRowArgs<IUnit>(_dataProvider, _prefabFactory, uiFactory, unitDetailsManager);

            _unitSection = GetComponentInChildren<UnitSection>();
            Assert.IsNotNull(_unitSection);
            _unitSection.Initialise(args, _itemStateManager);
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);

            hullsRowWrapper.HullsRow.OnPresenting(activationParameter: null);
            _buildingSection.OnPresented();
            _unitSection.OnPresented();
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

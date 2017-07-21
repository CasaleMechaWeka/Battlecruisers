using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.UnlockedItems;
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
		public UnlockedHullItemsRow unlockedHullsRow;
		public LoadoutBuildingItemsRow factoriesRow, defensivesRow, offensivesRow, tacticalsRow, ultrasRow;
		public UnlockedBuildingItemsRow unlockedFactoriesRow, unlockedDefensivesRow, unlockedOffensivesRow, unlockedTacticalsRow, unlockedUltrasRow;
		public BuildingDetailsManager buildingDetailsManager;
		public CruiserDetailsManager cruiserDetailsManager;

		public void Initialise(IScreensSceneGod screensSceneGod, IDataProvider dataProvider, IPrefabFactory prefabFactory)
		{
			base.Initialise(screensSceneGod);

			Assert.IsNotNull(dataProvider);
			Assert.IsNotNull(prefabFactory);

			_dataProvider = dataProvider;
			_gameModel = _dataProvider.GameModel;
			_prefabFactory = prefabFactory;

            buildingDetailsManager.Initialise();
            cruiserDetailsManager.Initialise();

			uiFactory.Initialise(buildingDetailsManager);

			// Initialise hull row
			new HullItemsRow(_gameModel, _prefabFactory, uiFactory, loadoutHullItem, unlockedHullsRow, cruiserDetailsManager);

			// Initialise building rows
			new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, BuildingCategory.Factory, factoriesRow, unlockedFactoriesRow, buildingDetailsManager);
			new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, BuildingCategory.Defence, defensivesRow, unlockedDefensivesRow, buildingDetailsManager);
			new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, BuildingCategory.Offence, offensivesRow, unlockedOffensivesRow, buildingDetailsManager);
			new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, BuildingCategory.Tactical, tacticalsRow, unlockedTacticalsRow, buildingDetailsManager);
			new BuildingItemsRow(_gameModel, _prefabFactory, uiFactory, BuildingCategory.Ultra, ultrasRow, unlockedUltrasRow, buildingDetailsManager);
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

using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cameras;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// === Tag keys :D ===
// FELIX    => Code todo
// TEMP     => Temporary for testing
// IPAD     => Update for IPad (usualy input related)
namespace BattleCruisers.Scenes
{
    /// <summary>
    /// Initialises everything :D
    /// </summary>
    public class BattleSceneGod : MonoBehaviour
	{
		private IDataProvider _dataProvider;
		private int _currentLevelNum;
		private Cruiser _playerCruiser, _aiCruiser;

		public UIManager uiManager;
		public UIFactory uiFactory;
		public BuildMenuController buildMenuController;
		public BuildableDetailsController buildableDetailsController;
        public InBattleCruiserDetailsController cruiserDetailsController;
		public ModalMenuController modalMenuController;
		public CameraController cameraController;
		public HealthBarController playerCruiserHealthBar, aiCruiserHealthBar;

		private const int CRUISER_OFFSET_IN_M = 35;

		void Awake()
		{
			Assert.raiseExceptions = true;
			Time.timeScale = 1;

            IDeferrer deferrer = GetComponent<IDeferrer>();

			Helper.AssertIsNotNull(uiManager, uiFactory, buildMenuController, buildableDetailsController, 
                modalMenuController, cameraController, playerCruiserHealthBar, aiCruiserHealthBar, deferrer);


			// TEMP  Only because I'm starting the Battle Scene without a previous Choose Level Scene
			if (ApplicationModel.SelectedLevel == -1)
			{
				ApplicationModel.SelectedLevel = 1;
			}


			_dataProvider = ApplicationModel.DataProvider;
			_currentLevelNum = ApplicationModel.SelectedLevel;

			ILoadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;
			ILevel currentLevel = _dataProvider.GetLevel(_currentLevelNum);


			// Common setup
			IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            ICruiserFactory cruiserFactory = new CruiserFactory(uiManager, prefabFactory, deferrer);


			// Instantiate player cruiser
			Cruiser playerCruiserPrefab = prefabFactory.GetCruiserPrefab(playerLoadout.Hull);
			_playerCruiser = prefabFactory.CreateCruiser(playerCruiserPrefab);
			_playerCruiser.transform.position = new Vector3(-CRUISER_OFFSET_IN_M, _playerCruiser.YAdjustmentInM, 0);


			// Instantiate AI cruiser
			Cruiser aiCruiserPrefab = prefabFactory.GetCruiserPrefab(currentLevel.Hull);
			_aiCruiser = prefabFactory.CreateCruiser(aiCruiserPrefab);

			_aiCruiser.transform.position = new Vector3(CRUISER_OFFSET_IN_M, _aiCruiser.YAdjustmentInM, 0);
			Quaternion rotation = _aiCruiser.transform.rotation;
			rotation.eulerAngles = new Vector3(0, 180, 0);
			_aiCruiser.transform.rotation = rotation;


			// UIManager
			uiManager.Initialise(_playerCruiser, _aiCruiser);


            // Initialise player cruiser
            cruiserFactory.InitialiseCruiser(_playerCruiser, _aiCruiser, playerCruiserHealthBar, Faction.Blues, Direction.Right);
			_playerCruiser.Destroyed += PlayerCruiser_Destroyed;


            // Initialise AI cruiser
            cruiserFactory.InitialiseCruiser(_aiCruiser, _playerCruiser, aiCruiserHealthBar, Faction.Reds, Direction.Left);
			_aiCruiser.Destroyed += AiCruiser_Destroyed;


			// UI
			ISpriteFetcher spriteFetcher = new SpriteFetcher();
            buildableDetailsController.Initialise(spriteFetcher, _playerCruiser.DroneManager, _playerCruiser.RepairManager);
            cruiserDetailsController.Initialise(_playerCruiser.DroneManager, _playerCruiser.RepairManager);
			uiFactory.Initialise(spriteFetcher, _playerCruiser.DroneManager);

            IBuildingGroupFactory buildingGroupFactory = new BuildingGroupFactory();
            IPrefabOrganiser prefabOrganiser = new PrefabOrganiser(playerLoadout, _playerCruiser.FactoryProvider, buildingGroupFactory);
			IList<IBuildingGroup> buildingGroups = prefabOrganiser.GetBuildingGroups();
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units = prefabOrganiser.GetUnits();
			buildMenuController.Initialise(buildingGroups, units);


			// Camera controller
			cameraController.Initialise(_playerCruiser, _aiCruiser);


            // TEMP  Disable AI
            //// AI
            //ILevelInfo levelInfo = new LevelInfo(_aiCruiser, _playerCruiser, _dataProvider.StaticData, prefabFactory, currentLevel.Num);
            //IAIManager aiManager = new AIManager(prefabFactory, deferrer, _dataProvider);
            //aiManager.CreateAI(levelInfo);
		}

		private void PlayerCruiser_Destroyed(object sender, DestroyedEventArgs e)
		{
			PauseGame();
			CompleteBattleAsLoss();
		}

		private void AiCruiser_Destroyed(object sender, DestroyedEventArgs e)
		{
			PauseGame();
			BattleResult victoryResult = new BattleResult(_currentLevelNum, wasVictory: true);
			CompleteBattle(victoryResult);
		}

		void Update()
		{
            // IPAD:  Adapt input for IPad :P
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				modalMenuController.ShowMenu(OnModalMenuDismissed);
				PauseGame();
			}
			// TEMP  Insta win :P
			else if (Input.GetKeyUp(KeyCode.W))
			{
				_aiCruiser.TakeDamage(_aiCruiser.Health);
			}
		}

		private void OnModalMenuDismissed(UserAction userAction)
		{
			switch (userAction)
			{
				case UserAction.Dismissed:
					ResumeGame();
					break;
				case UserAction.Quit:
					CompleteBattleAsLoss();
					break;
				default:
					throw new ArgumentException();
			}
		}

		private void PauseGame()
		{
			Time.timeScale = 0;
		}

		private void ResumeGame()
		{
			Time.timeScale = 1;
		}

		private void CompleteBattleAsLoss()
		{
			BattleResult lossResult = new BattleResult(_currentLevelNum, wasVictory: false);
			CompleteBattle(lossResult);
		}

		private void CompleteBattle(BattleResult battleResult)
		{
			CleanUp();

			_dataProvider.GameModel.LastBattleResult = battleResult;
			_dataProvider.SaveGame();

			ApplicationModel.ShowPostBattleScreen = true;

			SceneManager.LoadScene(SceneNames.SCREENS_SCENE);
		}

		private void CleanUp()
		{
			_playerCruiser.Destroyed -= PlayerCruiser_Destroyed;
			_aiCruiser.Destroyed -= AiCruiser_Destroyed;
		}
	}
}

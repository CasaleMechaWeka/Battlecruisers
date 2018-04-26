using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

// === Tag keys :D ===
// FELIX    => Code todo
// TEMP     => Temporary for testing
// IPAD     => Update for IPad (usualy input related)
// PERF     => Potential performance hit
namespace BattleCruisers.Scenes
{
    /// <summary>
    /// Initialises everything :D
    /// </summary>
    public class BattleSceneGod : MonoBehaviour
	{
        private ISceneNavigator _sceneNavigator;
		private IDataProvider _dataProvider;
		private int _currentLevelNum;
		private Cruiser _playerCruiser, _aiCruiser;

        public HUDCanvasController hudCanvas;
		public UIFactory uiFactory;
		public BuildMenuController buildMenuController;
		public ModalMenuController modalMenuController;
		public CameraController cameraController;
        public BackgroundController backgroundController;
        public NumOfDronesController numOfDronesController;

		private const int CRUISER_OFFSET_IN_M = 35;

		void Awake()
        {
            Assert.raiseExceptions = true;
            Time.timeScale = 1;

            IDeferrer deferrer = GetComponent<IDeferrer>();

            Helper.AssertIsNotNull(
                uiFactory,
                buildMenuController,
                hudCanvas,
                modalMenuController,
                cameraController,
                backgroundController,
                numOfDronesController,
                deferrer);


            // TEMP  Only because I'm starting the Battle Scene without a previous Choose Level Scene
            if (ApplicationModel.SelectedLevel == -1)
            {
                ApplicationModel.SelectedLevel = 1;
            }


            // TEMP  Forcing tutorial :)
            ApplicationModel.IsTutorial = true;


            _sceneNavigator = LandingSceneGod.SceneNavigator;
            _dataProvider = ApplicationModel.DataProvider;
            _currentLevelNum = ApplicationModel.SelectedLevel;


            // Common setup
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            ICruiserFactory cruiserFactory = new CruiserFactory(prefabFactory, deferrer, spriteProvider);
            IBattleSceneHelper helper = CreateHelper(prefabFactory, deferrer);


            // Instantiate player cruiser
            ILoadout playerLoadout = helper.GetPlayerLoadout();
            Cruiser playerCruiserPrefab = prefabFactory.GetCruiserPrefab(playerLoadout.Hull);
            _playerCruiser = prefabFactory.CreateCruiser(playerCruiserPrefab);
            _playerCruiser.transform.position = new Vector3(-CRUISER_OFFSET_IN_M, _playerCruiser.YAdjustmentInM, 0);


            // Instantiate AI cruiser
			ILevel currentLevel = _dataProvider.GetLevel(_currentLevelNum);
            Cruiser aiCruiserPrefab = prefabFactory.GetCruiserPrefab(currentLevel.Hull);
            _aiCruiser = prefabFactory.CreateCruiser(aiCruiserPrefab);

            _aiCruiser.transform.position = new Vector3(CRUISER_OFFSET_IN_M, _aiCruiser.YAdjustmentInM, 0);
            Quaternion rotation = _aiCruiser.transform.rotation;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            _aiCruiser.transform.rotation = rotation;


            // UIManager
            hudCanvas.StaticInitialise();
            IBuildableDetailsManager detailsManager = new BuildableDetailsManager(hudCanvas);
            IManagerArgs managerArgs
                = new ManagerArgs(
                    _playerCruiser,
                    _aiCruiser,
                    cameraController,
                    buildMenuController,
                    detailsManager);
            IUIManager uiManager = helper.CreateUIManager(managerArgs);
            backgroundController.Initialise(uiManager);


            // Initialise player cruiser
            cruiserFactory.InitialiseCruiser(_playerCruiser, _aiCruiser, uiManager, cameraController, Faction.Blues, Direction.Right);
            _playerCruiser.Destroyed += PlayerCruiser_Destroyed;
            hudCanvas.PlayerCruiserInfo.Initialise(_playerCruiser);


            // Initialise AI cruiser
            cruiserFactory.InitialiseCruiser(_aiCruiser, _playerCruiser, uiManager, cameraController, Faction.Reds, Direction.Left);
            _aiCruiser.Destroyed += AiCruiser_Destroyed;
            hudCanvas.AICruiserInfo.Initialise(_aiCruiser);


            // UI
            hudCanvas.Initialise(spriteProvider, _playerCruiser.DroneManager, _playerCruiser.RepairManager);
            uiFactory.Initialise(uiManager, spriteProvider, _playerCruiser.DroneManager);
            numOfDronesController.Initialise(_playerCruiser.DroneManager);

            IBuildingGroupFactory buildingGroupFactory = new BuildingGroupFactory();
            IPrefabOrganiser prefabOrganiser = new PrefabOrganiser(playerLoadout, _playerCruiser.FactoryProvider, buildingGroupFactory);
            IList<IBuildingGroup> buildingGroups = prefabOrganiser.GetBuildingGroups();
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units = prefabOrganiser.GetUnits();
            IBuildableSorterFactory sorterFactory = new BuildableSorterFactory();
            buildMenuController.Initialise(uiManager, uiFactory, buildingGroups, units, sorterFactory);

            uiManager.InitialUI();


            // Camera controller
            IMaterialFetcher materialFetcher = new MaterialFetcher();
            Material skyboxMaterial = materialFetcher.GetMaterial(currentLevel.SkyMaterialName);
            cameraController.Initialise(_playerCruiser, _aiCruiser, _dataProvider.SettingsManager, skyboxMaterial);


            helper.CreateAI(_aiCruiser, _playerCruiser, _currentLevelNum);
            GenerateClouds(currentLevel);
        }

        private IBattleSceneHelper CreateHelper(IPrefabFactory prefabFactory, IDeferrer deferrer)
        {
            if (ApplicationModel.IsTutorial)
            {
                return new TutorialHelper(_dataProvider);
            }
            else
            {
                return new NormalHelper(_dataProvider, prefabFactory, deferrer);
            }
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

        private void GenerateClouds(ILevel level)
        {
			CloudFactory cloudFactory = GetComponent<CloudFactory>();
			Assert.IsNotNull(cloudFactory);
			cloudFactory.Initialise();

            ICloudGenerator cloudGenerator = new CloudGenerator(cloudFactory);
            cloudGenerator.GenerateClouds(level.CloudStats);
        }

		void Update()
		{
            // IPAD:  Adapt input for IPad :P
			if (Input.GetKeyUp(KeyCode.Escape))
			{
                ShowModalMenu();
			}
			// TEMP  Insta win :P
			else if (Input.GetKeyUp(KeyCode.W))
			{
                _aiCruiser.TakeDamage(_aiCruiser.Health, damageSource: _playerCruiser);
			}
		}

        public void ShowModalMenu()
        {
            modalMenuController.ShowMenu(OnModalMenuDismissed);
            PauseGame();
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

            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE);
		}

		private void CleanUp()
		{
			_playerCruiser.Destroyed -= PlayerCruiser_Destroyed;
			_aiCruiser.Destroyed -= AiCruiser_Destroyed;
		}
	}
}

using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Tutorial;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using BattleCruisers.Utils.Threading;
using System;
using System.Collections.Generic;
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
        private ITutorialProvider _tutorialProvider;
		private INavigationSettings _navigationSettings;
        private IArtificialIntelligence _ai;
        private UserChosenTargetHighligher _userChosenTargetHighligher;

        public HUDCanvasController hudCanvas;
		public UIFactory uiFactory;
		public BuildMenuController buildMenuController;
		public ModalMenuController modalMenuController;
		public CameraInitialiser cameraInitialiser;
        public BackgroundController backgroundController;
        public NumOfDronesController numOfDronesController;

		private const int CRUISER_OFFSET_IN_M = 35;

		void Awake()
        {
            Assert.raiseExceptions = true;
            Time.timeScale = 1;

            IDeferrer deferrer = GetComponent<IDeferrer>();
            IVariableDelayDeferrer variableDelayDeferrer = GetComponent<IVariableDelayDeferrer>();
            IHighlightFactory highlightFactory = GetComponent<IHighlightFactory>();

            Helper.AssertIsNotNull(
                uiFactory,
                buildMenuController,
                hudCanvas,
                modalMenuController,
                cameraInitialiser,
                backgroundController,
                numOfDronesController,
                deferrer,
                variableDelayDeferrer,
                highlightFactory);


            // TEMP  Only because I'm starting the Battle Scene without a previous Choose Level Scene
            if (ApplicationModel.SelectedLevel == -1)
            {
                // TEMP  Force level I'm currently testing :)
                ApplicationModel.SelectedLevel = 13;
                //ApplicationModel.SelectedLevel = 1;
            }


            // TEMP  Forcing tutorial :)
            //ApplicationModel.IsTutorial = true;


            _sceneNavigator = LandingSceneGod.SceneNavigator;
            _dataProvider = ApplicationModel.DataProvider;
            _currentLevelNum = ApplicationModel.SelectedLevel;


            // Common setup
            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
            ICruiserFactory cruiserFactory = new CruiserFactory(prefabFactory, deferrer, variableDelayDeferrer, spriteProvider);
            IBattleSceneHelper helper = CreateHelper(prefabFactory, deferrer, variableDelayDeferrer);
            ISlotFilter highlightableSlotFilter = helper.CreateHighlightableSlotFilter();
			cameraInitialiser.StaticInitialise();
            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
            IUserChosenTargetManager aiCruiserUserChosenTargetManager = new DummyUserChosenTargetManager();


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
            IManagerArgs managerArgs
                = new ManagerArgs(
                    _playerCruiser,
                    _aiCruiser,
                    buildMenuController,
                    new BuildableDetailsManager(hudCanvas),
                    helper.CreateBuildingDeleteButtonFilter(_playerCruiser));
            IUIManager uiManager = helper.CreateUIManager(managerArgs);
            backgroundController.Initialise(uiManager);


            // Initialise player cruiser
			ICruiserHelper playerHelper = cruiserFactory.CreatePlayerHelper(uiManager, cameraInitialiser.CameraController);
            cruiserFactory
                .InitialiseCruiser(
                    _playerCruiser, 
                    _aiCruiser, 
                    uiManager, 
                    playerHelper, 
                    Faction.Blues, 
                    Direction.Right, 
                    highlightableSlotFilter, 
                    helper.PlayerCruiserBuildProgressCalculator,
                    playerCruiserUserChosenTargetManager);
            _playerCruiser.Destroyed += PlayerCruiser_Destroyed;


            // Initialise AI cruiser
			ICruiserHelper aiHelper = cruiserFactory.CreateAIHelper(uiManager, cameraInitialiser.CameraController);
            cruiserFactory
                .InitialiseCruiser(
                    _aiCruiser, 
                    _playerCruiser, 
                    uiManager, 
                    aiHelper, 
                    Faction.Reds, 
                    Direction.Left, 
                    highlightableSlotFilter, 
                    helper.AICruiserBuildProgressCalculator,
                    aiCruiserUserChosenTargetManager);
            _aiCruiser.Destroyed += AiCruiser_Destroyed;


			// UI
			_navigationSettings = new NavigationSettings();
			hudCanvas
                .Initialise(
                    spriteProvider, 
                    _playerCruiser, 
                    _aiCruiser, 
                    cameraInitialiser.CameraController, 
                    _navigationSettings.AreTransitionsEnabledFilter, 
                    playerCruiserUserChosenTargetManager);
            IBroadcastingFilter<IBuildable> buildableButtonShouldBeEnabledFilter = helper.CreateBuildableButtonFilter(_playerCruiser.DroneManager);
            IBroadcastingFilter<BuildingCategory> buildingCategoryButtonShouldBeEnabledFilter = helper.CreateCategoryButtonFilter();
            IBroadcastingFilter backButtonShouldBeEnabledFilter = helper.CreateBackButtonFilter();
            uiFactory.Initialise(uiManager, spriteProvider, buildableButtonShouldBeEnabledFilter, buildingCategoryButtonShouldBeEnabledFilter, backButtonShouldBeEnabledFilter);
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
            cameraInitialiser.Initialise(_playerCruiser, _aiCruiser, _dataProvider.SettingsManager, skyboxMaterial, _navigationSettings);
			cameraInitialiser.CameraController.FocusOnPlayerCruiser();


            // User chosen target highlighter
            IHighlightHelper highlightHelper = new HighlightHelper(highlightFactory);
            _userChosenTargetHighligher = new UserChosenTargetHighligher(playerCruiserUserChosenTargetManager, highlightHelper);


            _ai = helper.CreateAI(_aiCruiser, _playerCruiser, _currentLevelNum);
            GenerateClouds(currentLevel);


            StartTutorialIfNecessary(prefabFactory);
        }

        private IBattleSceneHelper CreateHelper(IPrefabFactory prefabFactory, IDeferrer deferrer, IVariableDelayDeferrer variableDelayDeferrer)
        {
            if (ApplicationModel.IsTutorial)
            {
                TutorialHelper helper = new TutorialHelper(_dataProvider, prefabFactory);
                _tutorialProvider = helper;
                return helper;
            }
            else
            {
                return new NormalHelper(_dataProvider, prefabFactory, deferrer, variableDelayDeferrer);
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

        private void StartTutorialIfNecessary(IPrefabFactory prefabFactory)
        {
            if (ApplicationModel.IsTutorial)
            {
				_dataProvider.GameModel.LastBattleResult = null;
                _dataProvider.GameModel.HasAttemptedTutorial = true;
                _dataProvider.SaveGame();

                ITutorialArgs tutorialArgs 
				    = new TutorialArgs(
					    _playerCruiser, 
    					_aiCruiser, 
    					hudCanvas, 
    					buildMenuController, 
    					_tutorialProvider, 
    					prefabFactory, 
    					_navigationSettings,
					    cameraInitialiser.CameraController,
					    cameraInitialiser.UserInputCameraMover);

                TutorialManager tutorialManager = GetComponentInChildren<TutorialManager>();
                Assert.IsNotNull(tutorialManager);
                tutorialManager.Initialise(tutorialArgs);

                tutorialManager.TutorialCompleted += _tutorialManager_TutorialCompleted;
				tutorialManager.StartTutorial();
            }
        }

        private void _tutorialManager_TutorialCompleted(object sender, EventArgs e)
        {
            InstaWin();
        }

		void Update()
		{
			cameraInitialiser.CameraController.MoveCamera(Time.deltaTime);

            // IPAD:  Adapt input for IPad :P
			if (Input.GetKeyUp(KeyCode.Escape))
			{
                ShowModalMenu();
			}
			// TEMP  Insta win :P
			else if (Input.GetKeyUp(KeyCode.W))
			{
                InstaWin();
            }
        }

        private void InstaWin()
        {
            _aiCruiser.TakeDamage(_aiCruiser.Health, damageSource: _playerCruiser);
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

            if (!ApplicationModel.IsTutorial)
            {
                // Completing the tutorial does not count as a real level, so 
                // only save save battle result if this was not the tutorial.
				_dataProvider.GameModel.LastBattleResult = battleResult;
				_dataProvider.SaveGame();
            }

            ApplicationModel.IsTutorial = false;
			ApplicationModel.ShowPostBattleScreen = true;

            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE);
		}

		private void CleanUp()
		{
			_playerCruiser.Destroyed -= PlayerCruiser_Destroyed;
			_aiCruiser.Destroyed -= AiCruiser_Destroyed;
            _ai.DisposeManagedState();
            _userChosenTargetHighligher.DisposeManagedState();
		}
	}
}

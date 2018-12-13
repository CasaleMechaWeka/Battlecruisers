// FELIX  Delete this & scene :)
//using BattleCruisers.AI;
//using BattleCruisers.Buildables;
//using BattleCruisers.Buildables.Buildings;
//using BattleCruisers.Buildables.Units;
//using BattleCruisers.Cruisers;
//using BattleCruisers.Cruisers.Construction;
//using BattleCruisers.Cruisers.Damage;
//using BattleCruisers.Cruisers.Helpers;
//using BattleCruisers.Cruisers.Slots;
//using BattleCruisers.Data;
//using BattleCruisers.Data.Models;
//using BattleCruisers.Targets.TargetTrackers;
//using BattleCruisers.Targets.TargetTrackers.UserChosen;
//using BattleCruisers.Tutorial;
//using BattleCruisers.Tutorial.Highlighting;
//using BattleCruisers.UI.BattleScene;
//using BattleCruisers.UI.BattleScene.BuildMenus;
//using BattleCruisers.UI.BattleScene.Buttons.Filters;
//using BattleCruisers.UI.BattleScene.Clouds;
//using BattleCruisers.UI.BattleScene.Cruisers;
//using BattleCruisers.UI.BattleScene.Manager;
//using BattleCruisers.UI.BattleScene.Navigation;
//using BattleCruisers.UI.Cameras;
//using BattleCruisers.UI.Cameras.Helpers;
//using BattleCruisers.UI.Common.BuildableDetails;
//using BattleCruisers.UI.Music;
//using BattleCruisers.Utils;
//using BattleCruisers.Utils.BattleScene;
//using BattleCruisers.Utils.Fetchers;
//using BattleCruisers.Utils.PlatformAbstractions;
//using BattleCruisers.Utils.PlatformAbstractions.UI;
//using BattleCruisers.Utils.Sorting;
//using BattleCruisers.Utils.Threading;
//using NSubstitute;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Assertions;

//namespace BattleCruisers.Scenes.BattleScene
//{
//    /// <summary>
//    /// Initialises everything :D
//    /// </summary>
//    public class BattleSceneGod : MonoBehaviour
//	{
//        private ISceneNavigator _sceneNavigator;
//        private IApplicationModel _applicationModel;
//        private IDataProvider _dataProvider;
//		private int _currentLevelNum;
//		private Cruiser _playerCruiser, _aiCruiser;
//        private ITutorialProvider _tutorialProvider;
//		private INavigationSettings _navigationSettings;
//        private IArtificialIntelligence _ai;
//        private UserChosenTargetHighligher _userChosenTargetHighligher;
//        private IPauseGameManager _pauseGameManager;
//        private CruiserEventMonitor _cruiserEventMonitor;
//        private IManagedDisposable _droneEventSoundPlayer;
//        private UltrasConstructionMonitor _ultrasConstructionMonitor;
//        // Just holding a reference so this does not get garbage collected.
//#pragma warning disable CS0414  // Variable is assigned but never used
//        private DangerMusicPlayer _dangerMusicPlayer;
//#pragma warning restore CS0414  // Variable is assigned but never used
//        private IAudioSource _audioSource;
//        private IBattleCompletionHandler _battleCompletionHandler;

//        public HUDCanvasController hudCanvas;
//        public BuildMenuController buildMenuController;
//		public ModalMenuController modalMenuController;
//		public CameraInitialiser cameraInitialiser;
//        public BackgroundController backgroundController;
//        public NumOfDronesController numOfDronesController;

//		private const int CRUISER_OFFSET_IN_M = 35;

//		void Awake()
//        {
//            Assert.raiseExceptions = true;
//            Time.timeScale = 1;

//            IDeferrer deferrer = GetComponent<IDeferrer>();
//            IVariableDelayDeferrer variableDelayDeferrer = GetComponent<IVariableDelayDeferrer>();
//            IHighlightFactory highlightFactory = GetComponent<IHighlightFactory>();

//            AudioSource audioSource = GetComponent<AudioSource>();
//            Assert.IsNotNull(audioSource);
//            _audioSource = new AudioSourceBC(audioSource);

//            Helper.AssertIsNotNull(
//                buildMenuController,
//                hudCanvas,
//                modalMenuController,
//                cameraInitialiser,
//                backgroundController,
//                numOfDronesController,
//                deferrer,
//                variableDelayDeferrer,
//                highlightFactory);


//            _sceneNavigator = LandingSceneGod.SceneNavigator;
//            IMusicPlayer musicPlayer = LandingSceneGod.MusicPlayer;

//            _applicationModel = ApplicationModelProvider.ApplicationModel;

//            // TEMP  Only because I'm starting the Battle Scene without a previous Choose Level Scene
//            if (_applicationModel.SelectedLevel == -1)
//            {
//                // TEMP  Force level I'm currently testing :)
//                _applicationModel.SelectedLevel = 1;

//                musicPlayer = Substitute.For<IMusicPlayer>();
//                _sceneNavigator = Substitute.For<ISceneNavigator>();
//            }


//            _dataProvider = _applicationModel.DataProvider;
//            _currentLevelNum = _applicationModel.SelectedLevel;
//            musicPlayer.PlayBattleSceneMusic();
//            _battleCompletionHandler = new BattleCompletionHandler(_applicationModel, _sceneNavigator);


//            // TEMP  Forcing tutorial :)
//            //ApplicationModel.IsTutorial = true;


//            // Common setup
//            IPrefabFactory prefabFactory = new PrefabFactory(new PrefabFetcher());
//            ISpriteProvider spriteProvider = new SpriteProvider(new SpriteFetcher());
//            IBattleSceneHelper helper = CreateHelper(prefabFactory, variableDelayDeferrer);
//            ISlotFilter highlightableSlotFilter = helper.CreateHighlightableSlotFilter();
//			cameraInitialiser.StaticInitialise();
//            IUserChosenTargetManager playerCruiserUserChosenTargetManager = new UserChosenTargetManager();
//            IUserChosenTargetManager aiCruiserUserChosenTargetManager = new DummyUserChosenTargetManager();
//            ITime time = new TimeBC();
//            _pauseGameManager = new PauseGameManager(time);
//            modalMenuController.Initialise(_applicationModel.IsTutorial);


//            // Instantiate player cruiser
//            ILoadout playerLoadout = helper.GetPlayerLoadout();
//            Cruiser playerCruiserPrefab = prefabFactory.GetCruiserPrefab(playerLoadout.Hull);
//            _playerCruiser = prefabFactory.CreateCruiser(playerCruiserPrefab);
//            _playerCruiser.transform.position = new Vector3(-CRUISER_OFFSET_IN_M, _playerCruiser.YAdjustmentInM, 0);


//            // Instantiate AI cruiser
//			ILevel currentLevel = _dataProvider.GetLevel(_currentLevelNum);
//            Cruiser aiCruiserPrefab = prefabFactory.GetCruiserPrefab(currentLevel.Hull);
//            _aiCruiser = prefabFactory.CreateCruiser(aiCruiserPrefab);

//            _aiCruiser.transform.position = new Vector3(CRUISER_OFFSET_IN_M, _aiCruiser.YAdjustmentInM, 0);
//            Quaternion rotation = _aiCruiser.transform.rotation;
//            rotation.eulerAngles = new Vector3(0, 180, 0);
//            _aiCruiser.transform.rotation = rotation;


//            // UIManager
//            hudCanvas.StaticInitialise();
//            IManagerArgs managerArgs
//                = new ManagerArgs(
//                    _playerCruiser,
//                    _aiCruiser,
//                    buildMenuController,
//                    new ItemDetailsManager(hudCanvas));
//            // NEWUI Everythings' broken anyway :/
//            //IUIManager uiManager = helper.CreateUIManager(managerArgs);
//            IUIManager uiManager = null;
//            backgroundController.Initialise(uiManager);


//            // Initialise player cruiser
//            ICruiserFactory cruiserFactory 
//                = new CruiserFactory(
//                    prefabFactory, 
//                    deferrer, 
//                    variableDelayDeferrer, 
//                    spriteProvider, 
//                    _playerCruiser, 
//                    _aiCruiser, 
//                    cameraInitialiser.MainCamera,
//                    _audioSource);
//			ICruiserHelper playerHelper = cruiserFactory.CreatePlayerHelper(uiManager, cameraInitialiser.CameraController);
//            cruiserFactory
//                .InitialisePlayerCruiser(
//                    uiManager,
//                    playerHelper,
//                    highlightableSlotFilter,
//                    helper.PlayerCruiserBuildProgressCalculator,
//                    playerCruiserUserChosenTargetManager);
//            _playerCruiser.Destroyed += PlayerCruiser_Destroyed;


//            // Initialise AI cruiser
//            IUserChosenTargetHelper userChosenTargetHelper 
//                = new UserChosenTargetHelper(
//                    playerCruiserUserChosenTargetManager, 
//                    _playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
//            ICruiserHelper aiHelper = cruiserFactory.CreateAIHelper(uiManager, cameraInitialiser.CameraController);
//            cruiserFactory
//                .InitialiseAICruiser(
//                    uiManager,
//                    aiHelper,
//                    highlightableSlotFilter,
//                    helper.AICruiserBuildProgressCalculator,
//                    aiCruiserUserChosenTargetManager,
//                    userChosenTargetHelper);
//            _aiCruiser.Destroyed += AiCruiser_Destroyed;


//			// UI
//			_navigationSettings = new NavigationSettings();
//            IButtonVisibilityFilters buttonVisibilityFilters = helper.CreateButtonVisibilityFilters(_playerCruiser.DroneManager);

//            hudCanvas
//                .Initialise(
//                    _playerCruiser,
//                    _aiCruiser,
//                    cameraInitialiser.CameraController,
//                    _navigationSettings.AreTransitionsEnabledFilter,
//                    userChosenTargetHelper,
//                    buttonVisibilityFilters.ChooseTargetButtonVisiblityFilter,
//                    buttonVisibilityFilters.DeletButtonVisiblityFilter);
//            numOfDronesController.Initialise(_playerCruiser.DroneManager);

//            IBuildingGroupFactory buildingGroupFactory = new BuildingGroupFactory();
//            IPrefabOrganiser prefabOrganiser = new PrefabOrganiser(playerLoadout, prefabFactory, buildingGroupFactory);
//            IList<IBuildingGroup> buildingGroups = prefabOrganiser.GetBuildingGroups();
//            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units = prefabOrganiser.GetUnits();
//            IBuildableSorterFactory sorterFactory = new BuildableSorterFactory();
//            IPlayerCruiserFocusHelper playerCruiserFocusHelper
//                = new PlayerCruiserFocusHelper(
//                    cameraInitialiser.MainCamera,
//                    cameraInitialiser.CameraController,
//                    _playerCruiser);

//            buildMenuController
//                .Initialise(
//                    uiManager,
//                    buildingGroups,
//                    units,
//                    sorterFactory,
//                    buttonVisibilityFilters,
//                    spriteProvider,
//                    playerCruiserFocusHelper,
//                    helper.GetBuildableButtonSoundPlayer(_playerCruiser));

//            uiManager.InitialUI();


//			// Camera controller
//            IMaterialFetcher materialFetcher = new MaterialFetcher();
//            Material skyboxMaterial = materialFetcher.GetMaterial(currentLevel.SkyMaterialName);
//            cameraInitialiser.Initialise(_playerCruiser, _aiCruiser, _dataProvider.SettingsManager, skyboxMaterial, _navigationSettings, _pauseGameManager);
//			cameraInitialiser.CameraController.FocusOnPlayerCruiser();


//            // User chosen target highlighter
//            IHighlightHelper highlightHelper = new HighlightHelper(highlightFactory);
//            _userChosenTargetHighligher = new UserChosenTargetHighligher(playerCruiserUserChosenTargetManager, highlightHelper);


//            _ai = helper.CreateAI(_aiCruiser, _playerCruiser, _currentLevelNum);
//            GenerateClouds(currentLevel);
//            _cruiserEventMonitor = CreateCruiserEventMonitor(_playerCruiser, time);
//            _droneEventSoundPlayer = helper.CreateDroneEventSoundPlayer(_playerCruiser, variableDelayDeferrer);
//            _ultrasConstructionMonitor = CreateUltrasConstructionMonitor(_aiCruiser);
//            _dangerMusicPlayer = CreateDangerMusicPlayer(musicPlayer, _playerCruiser, _aiCruiser, variableDelayDeferrer);

//            StartTutorialIfNecessary(prefabFactory);
//        }

//        private CruiserEventMonitor CreateCruiserEventMonitor(ICruiser playerCruiser, ITime time)
//        {
//            return
//                new CruiserEventMonitor(
//                    new HealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
//                    new CruiserDamagedMonitorDebouncer(
//                        new CruiserDamageMonitor(playerCruiser),
//                        time),
//                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
//        }

//        private UltrasConstructionMonitor CreateUltrasConstructionMonitor(ICruiser aiCruiser)
//        {
//            return
//                new UltrasConstructionMonitor(
//                    aiCruiser,
//                    aiCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
//        }

//        private DangerMusicPlayer CreateDangerMusicPlayer(
//            IMusicPlayer musicPlayer,
//            ICruiser playerCruiser, 
//            ICruiser aiCruiser,
//            IVariableDelayDeferrer deferrer)
//        {
//            return
//                new DangerMusicPlayer(
//                    musicPlayer,
//                    new DangerMonitor(
//                        playerCruiser,
//                        aiCruiser,
//                        new HealthThresholdMonitor(playerCruiser, thresholdProportion: 0.3f),
//                        new HealthThresholdMonitor(aiCruiser, thresholdProportion: 0.3f)),
//                    deferrer);
//        }

//        private IBattleSceneHelper CreateHelper(IPrefabFactory prefabFactory, IVariableDelayDeferrer variableDelayDeferrer)
//        {
//            if (_applicationModel.IsTutorial)
//            {
//                TutorialHelper helper = new TutorialHelper(_dataProvider, prefabFactory);
//                _tutorialProvider = helper;
//                return helper;
//            }
//            else
//            {
//                return new NormalHelper(_dataProvider, prefabFactory, variableDelayDeferrer);
//            }
//        }

//        private void PlayerCruiser_Destroyed(object sender, DestroyedEventArgs e)
//		{
//            _pauseGameManager.PauseGame();
//            CompleteBattle(wasVictory: false);
//		}

//		private void AiCruiser_Destroyed(object sender, DestroyedEventArgs e)
//		{
//            _pauseGameManager.PauseGame();
//            CompleteBattle(wasVictory: true);
//        }

//        private void GenerateClouds(ILevel level)
//        {
//			CloudFactory cloudFactory = GetComponent<CloudFactory>();
//			Assert.IsNotNull(cloudFactory);
//			cloudFactory.Initialise();

//            ICloudGenerator cloudGenerator = new CloudGenerator(cloudFactory);
//            cloudGenerator.GenerateClouds(level.CloudStats);
//        }

//        private void StartTutorialIfNecessary(IPrefabFactory prefabFactory)
//        {
//            if (_applicationModel.IsTutorial)
//            {
//				_dataProvider.GameModel.LastBattleResult = null;
//                _dataProvider.GameModel.HasAttemptedTutorial = true;
//                _dataProvider.SaveGame();

//                ITutorialArgs tutorialArgs 
//				    = new TutorialArgs(
//					    _playerCruiser, 
//    					_aiCruiser, 
//    					hudCanvas, 
//    					buildMenuController,
//    					_tutorialProvider, 
//    					prefabFactory, 
//    					_navigationSettings,
//					    cameraInitialiser.CameraController,
//					    cameraInitialiser.UserInputCameraMover);

//                TutorialManager tutorialManager = GetComponentInChildren<TutorialManager>();
//                Assert.IsNotNull(tutorialManager);
//                tutorialManager.Initialise(tutorialArgs);
//				tutorialManager.StartTutorial();
//            }
//        }

//		void Update()
//		{
//			cameraInitialiser.CameraController.MoveCamera(Time.deltaTime);

//            // IPAD:  Adapt input for IPad :P
//			if (Input.GetKeyUp(KeyCode.Escape))
//			{
//                ShowModalMenu();
//			}
//			else if (Input.GetKeyUp(KeyCode.W) && Debug.isDebugBuild)
//			{
//                InstaWin();
//            }
//        }

//        private void InstaWin()
//        {
//            _aiCruiser.TakeDamage(_aiCruiser.Health, damageSource: _playerCruiser);
//        }

//        public void ShowModalMenu()
//        {
//            modalMenuController.ShowMenu(OnModalMenuDismissed);
//            _pauseGameManager.PauseGame();
//        }

//		private void OnModalMenuDismissed(UserAction userAction)
//		{
//			switch (userAction)
//			{
//				case UserAction.Dismissed:
//                    _pauseGameManager.ResumeGame();
//					break;
//				case UserAction.Quit:
//                    CompleteBattle(wasVictory: false);
//					break;
//				default:
//					throw new ArgumentException();
//			}
//		}

//		private void CompleteBattle(bool wasVictory)
//		{
//			CleanUp();
//            _battleCompletionHandler.CompleteBattle(wasVictory);
//		}

//		private void CleanUp()
//		{
//			_playerCruiser.Destroyed -= PlayerCruiser_Destroyed;
//			_aiCruiser.Destroyed -= AiCruiser_Destroyed;
//            _ai.DisposeManagedState();
//            _userChosenTargetHighligher.DisposeManagedState();
//            _cruiserEventMonitor.DisposeManagedState();
//            _droneEventSoundPlayer.DisposeManagedState();
//            _ultrasConstructionMonitor.DisposeManagedState();
//		}
//	}
//}

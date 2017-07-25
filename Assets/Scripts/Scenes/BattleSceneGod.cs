using System;
using System.Collections.Generic;
using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cameras;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.UI.Common.BuildingDetails;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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
		private BuildingGroupFactory _buildingGroupFactory;
		private Bot _bot;

		public UIManager uiManager;
		public UIFactory uiFactory;
		public BuildMenuController buildMenuController;
		public BuildableDetailsController buildableDetailsController;
		public ModalMenuController modalMenuController;
		public CameraController cameraController;
		public HealthBarController playerCruiserHealthBar, aiCruiserHealthBar;
		
		// User needs to be able to build at least one building
		private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
		// Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
		private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

		private const int CRUISER_OFFSET_IN_M = 35;

		void Awake()
		{
			Assert.raiseExceptions = true;


			// FELIX  TEMP  Only because I'm starting the Battle Scene without a previous Choose Level Scene
			if (ApplicationModel.SelectedLevel == -1)
			{
				ApplicationModel.SelectedLevel = 1;
			}


			_dataProvider = ApplicationModel.DataProvider;
			_currentLevelNum = ApplicationModel.SelectedLevel;

			Loadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;
			ILevel currentLevel = _dataProvider.GetLevel(_currentLevelNum);
			Loadout aiLoadout = currentLevel.AiLoadout;


			// Common setup
			_buildingGroupFactory = new BuildingGroupFactory();
			IPrefabFactory prefabFactory  = new PrefabFactory(new PrefabFetcher());


			// Instantiate player cruiser
			Cruiser playerCruiserPrefab = prefabFactory.GetCruiserPrefab(playerLoadout.Hull);
			_playerCruiser = prefabFactory.CreateCruiser(playerCruiserPrefab);
			_playerCruiser.transform.position = new Vector3(-CRUISER_OFFSET_IN_M, _playerCruiser.YAdjustmentInM, 0);


			// Instantiate AI cruiser
			Cruiser aiCruiserPrefab = prefabFactory.GetCruiserPrefab(aiLoadout.Hull);
			_aiCruiser = prefabFactory.CreateCruiser(aiCruiserPrefab);
			
			_aiCruiser.transform.position = new Vector3(CRUISER_OFFSET_IN_M, _aiCruiser.YAdjustmentInM, 0);
			Quaternion rotation = _aiCruiser.transform.rotation;
			rotation.eulerAngles = new Vector3(0, 180, 0);
			_aiCruiser.transform.rotation = rotation;


			// UIManager
			uiManager.Initialise(_playerCruiser, _aiCruiser);


			// Initialise player cruiser
			IFactoryProvider playerFactoryProvider = new FactoryProvider(prefabFactory, _playerCruiser, _aiCruiser);
			IDroneManager playerDroneManager = new DroneManager();
			IDroneConsumerProvider playerDroneConsumerProvider = new DroneConsumerProvider(playerDroneManager);
			_playerCruiser.Initialise(Faction.Blues, _aiCruiser, playerCruiserHealthBar, uiManager, playerDroneManager, 
				playerDroneConsumerProvider, playerFactoryProvider, Direction.Right);
			_playerCruiser.Destroyed += PlayerCruiser_Destroyed;


			// Initialise AI cruiser
			IFactoryProvider aiFactoryProvider = new FactoryProvider(prefabFactory, _aiCruiser, _playerCruiser);
			IDroneManager aiDroneManager = new DroneManager();
			IDroneConsumerProvider aiDroneConsumerProvider = new DroneConsumerProvider(aiDroneManager);
			_aiCruiser.Initialise(Faction.Reds, _playerCruiser, aiCruiserHealthBar, uiManager, aiDroneManager, 
				aiDroneConsumerProvider, aiFactoryProvider, Direction.Left);
			_aiCruiser.Destroyed += AiCruiser_Destroyed;


			// UI
			ISpriteFetcher spriteFetcher = new SpriteFetcher();
			buildableDetailsController.Initialise(playerDroneManager, spriteFetcher);
			uiFactory.Initialise(spriteFetcher, playerDroneManager);
			
			IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> buildings = GetBuildingsFromKeys(playerLoadout, playerFactoryProvider);
			IList<BuildingGroup> buildingGroups = CreateBuildingGroups(buildings);
			IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units = GetUnitsFromKeys(playerLoadout, playerFactoryProvider);
			buildMenuController.Initialise(buildingGroups, units);
			
			
			// Camera controller
			cameraController.Initialise(_playerCruiser, _aiCruiser);


			// AI
			IList<BuildingKey> buildOrder = GetBuildOrder();
			_bot = new Bot(_aiCruiser, _playerCruiser, buildOrder, prefabFactory);
//			Invoke("StartBot", 10);
//			Invoke("StartBot", 2);
		}

		private void StartBot()
		{
			_bot.Start();
		}

		private IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> GetBuildingsFromKeys(Loadout loadout, IFactoryProvider factoryProvider)
		{
			IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings = new Dictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>>();
			
			foreach (BuildingCategory category in Enum.GetValues(typeof(BuildingCategory)))
			{
				IList<BuildingKey> buildingKeys = loadout.GetBuildings(category);
				
				if (buildingKeys.Count != 0)
				{
					IList<IBuildableWrapper<IBuilding>> buildings = new List<IBuildableWrapper<IBuilding>>();
					categoryToBuildings[category] = buildings;
					
					foreach (BuildingKey buildingKey in buildingKeys)
					{
                        IBuildableWrapper<IBuilding> buildingWrapper = factoryProvider.PrefabFactory.GetBuildingWrapperPrefab(buildingKey).UnityObject;
						categoryToBuildings[buildingWrapper.Buildable.Category].Add(buildingWrapper);
					}
				}
			}

			return categoryToBuildings;
		}

		private IList<BuildingGroup> CreateBuildingGroups(IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> buildingCategoryToGroups)
		{
			IList<BuildingGroup> buildingGroups = new List<BuildingGroup>();

			foreach (KeyValuePair<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings in buildingCategoryToGroups)
			{
				BuildingGroup group = _buildingGroupFactory.CreateBuildingGroup(categoryToBuildings.Key, categoryToBuildings.Value);
				buildingGroups.Add(group);
			}
			
			if (buildingGroups.Count < MIN_NUM_OF_BUILDING_GROUPS
				|| buildingGroups.Count > MAX_NUM_OF_BUILDING_GROUPS)
			{
				throw new InvalidProgramException();
			}
			
			return buildingGroups;
		}
	
		private IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> GetUnitsFromKeys(Loadout loadout, IFactoryProvider factoryProvider)
		{
			IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> categoryToUnits = new Dictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>>();

			foreach (UnitCategory unitCategory in Enum.GetValues(typeof(UnitCategory)))
			{
				IList<UnitKey> unitKeys = loadout.GetUnits(unitCategory);

				if (unitKeys.Count != 0)
				{
					categoryToUnits[unitCategory] = GetUnits(unitKeys, factoryProvider);
				}
			}

			return categoryToUnits;
		}

		private IList<IBuildableWrapper<IUnit>> GetUnits(IList<UnitKey> unitKeys, IFactoryProvider factoryProvider)
		{
			IList<IBuildableWrapper<IUnit>> unitWrappers = new List<IBuildableWrapper<IUnit>>(unitKeys.Count);

			foreach (UnitKey unitKey in unitKeys)
			{
				IBuildableWrapper<IUnit> unitWrapper = factoryProvider.PrefabFactory.GetUnitWrapperPrefab(unitKey);
				unitWrappers.Add(unitWrapper);
			}

			return unitWrappers;
		}

		private IList<BuildingKey> GetBuildOrder()
		{
			IList<BuildingKey> buildOrder = new List<BuildingKey>();

			buildOrder.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "TeslaCoil"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "Mortar"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Factory, "NavalFactory"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"));
//			buildOrder.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"));

			return buildOrder;
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
			// FELIX  Adapt for IPad :P
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				modalMenuController.ShowMenu(OnModalMenuDismissed);
				PauseGame();
			}
			// FELIX  TEMP  Insta win :P
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

using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Fetchers.PrefabKeys;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.BuildingDetails;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Units.Aircraft.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
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
		private Cruiser _playerCruiser, _aiCruiser;
		private BuildingGroupFactory _buildingGroupFactory;
		private Bot _bot;

		public PrefabFactory prefabFactory;
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
			if (ApplicationModel.SelectedLevel == 0)
			{
				ApplicationModel.SelectedLevel = 1;
			}



			IDataProvider dataProvider = ApplicationModel.DataProvider;
			Loadout playerLoadout = dataProvider.GameModel.PlayerLoadout;
			ILevel currentLevel = dataProvider.GetLevel(ApplicationModel.SelectedLevel);
			Loadout aiLoadout = currentLevel.AiLoadout;


			// Common setup
			_buildingGroupFactory = new BuildingGroupFactory();
			PrefabFetcher prefabFetcher = new PrefabFetcher();
			prefabFactory.Initialise(prefabFetcher);


			// Instantiate player cruiser
			Cruiser playerCruiserPrefab = prefabFactory.GetCruiserPrefab(playerLoadout.Hull);
			_playerCruiser = prefabFactory.CreateCruiser(playerCruiserPrefab);
			_playerCruiser.transform.position = new Vector3(-CRUISER_OFFSET_IN_M, 0, 0);


			// Instantiate AI cruiser
			Cruiser aiCruiserPrefab = prefabFactory.GetCruiserPrefab(aiLoadout.Hull);
			_aiCruiser = prefabFactory.CreateCruiser(aiCruiserPrefab);
			
			_aiCruiser.transform.position = new Vector3(CRUISER_OFFSET_IN_M, 0, 0);
			Quaternion rotation = _aiCruiser.transform.rotation;
			rotation.eulerAngles = new Vector3(0, 180, 0);
			_aiCruiser.transform.rotation = rotation;


			// UIManager
			uiManager.Initialise(_playerCruiser, _aiCruiser);


			// Initialise layer cruiser
			ITargetsFactory playerCruiserTargetsFactory = new TargetsFactory(_aiCruiser);
			IAircraftProvider playerCruiserAircraftProvider = new AircraftProvider(_playerCruiser.transform.position, _aiCruiser.transform.position);
			IDroneManager playerDroneManager = new DroneManager();
			IDroneConsumerProvider playerDroneConsumerProvider = new DroneConsumerProvider(playerDroneManager);
			_playerCruiser.Initialise(Faction.Blues, _aiCruiser, playerCruiserHealthBar, uiManager, playerDroneManager, playerDroneConsumerProvider, playerCruiserTargetsFactory, playerCruiserAircraftProvider, prefabFactory, Direction.Right);


			// Initialise AI cruiser
			ITargetsFactory aiCruiserTargetsFactory = new TargetsFactory(_playerCruiser);
			IAircraftProvider aiCruiserAircraftProvider = new AircraftProvider(_aiCruiser.transform.position, _playerCruiser.transform.position);
			IDroneManager aiDroneManager = new DroneManager();
			IDroneConsumerProvider aiDroneConsumerProvider = new DroneConsumerProvider(aiDroneManager);
			_aiCruiser.Initialise(Faction.Reds, _playerCruiser, aiCruiserHealthBar, uiManager, aiDroneManager, aiDroneConsumerProvider, aiCruiserTargetsFactory, aiCruiserAircraftProvider, prefabFactory, Direction.Left);


			// UI
			ISpriteFetcher spriteFetcher = new SpriteFetcher();
			buildableDetailsController.Initialise(playerDroneManager, spriteFetcher);
			uiFactory.Initialise(spriteFetcher, playerDroneManager);
			
			IDictionary<BuildingCategory, IList<BuildingWrapper>> buildings = GetBuildingsFromKeys(playerLoadout, _playerCruiser, _aiCruiser);
			IList<BuildingGroup> buildingGroups = CreateBuildingGroups(buildings);
			IDictionary<UnitCategory, IList<UnitWrapper>> units = GetUnitsFromKeys(playerLoadout, _playerCruiser, _aiCruiser);
			buildMenuController.Initialise(buildingGroups, units);
			
			
			// Camera controller
			cameraController.Initialise(_playerCruiser.GameObject, _aiCruiser.GameObject);


			// AI
			IList<BuildingKey> buildOrder = GetBuildOrder();
			_bot = new Bot(_aiCruiser, _playerCruiser, buildOrder, prefabFactory);
//			Invoke("StartBot", 10);
			Invoke("StartBot", 2);
		}

		private void StartBot()
		{
			_bot.Start();
		}

		private IDictionary<BuildingCategory, IList<BuildingWrapper>> GetBuildingsFromKeys(Loadout loadout, Cruiser parentCruiser, Cruiser hostileCruiser)
		{
			IDictionary<BuildingCategory, IList<BuildingWrapper>> categoryToBuildings = new Dictionary<BuildingCategory, IList<BuildingWrapper>>();
			
			foreach (BuildingCategory category in Enum.GetValues(typeof(BuildingCategory)))
			{
				IList<BuildingKey> buildingKeys = loadout.GetBuildings(category);
				
				if (buildingKeys.Count != 0)
				{
					IList<BuildingWrapper> buildings = new List<BuildingWrapper>();
					categoryToBuildings[category] = buildings;
					
					foreach (BuildingKey buildingKey in buildingKeys)
					{
						BuildingWrapper buildingWrapper = prefabFactory.GetBuildingWrapperPrefab(buildingKey);
						categoryToBuildings[buildingWrapper.building.category].Add(buildingWrapper);
					}
				}
			}

			return categoryToBuildings;
		}

		private IList<BuildingGroup> CreateBuildingGroups(IDictionary<BuildingCategory, IList<BuildingWrapper>> buildingCategoryToGroups)
		{
			IList<BuildingGroup> buildingGroups = new List<BuildingGroup>();

			foreach (KeyValuePair<BuildingCategory, IList<BuildingWrapper>> categoryToBuildings in buildingCategoryToGroups)
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
	
		private IDictionary<UnitCategory, IList<UnitWrapper>> GetUnitsFromKeys(Loadout loadout, Cruiser parentCruiser, Cruiser hostileCruiser)
		{
			IDictionary<UnitCategory, IList<UnitWrapper>> categoryToUnits = new Dictionary<UnitCategory, IList<UnitWrapper>>();

			foreach (UnitCategory unitCategory in Enum.GetValues(typeof(UnitCategory)))
			{
				IList<UnitKey> unitKeys = loadout.GetUnits(unitCategory);

				if (unitKeys.Count != 0)
				{
					categoryToUnits[unitCategory] = GetUnits(unitKeys, parentCruiser, hostileCruiser);
				}
			}

			return categoryToUnits;
		}

		private IList<UnitWrapper> GetUnits(IList<UnitKey> unitKeys, Cruiser parentCruiser, Cruiser hostileCruiser)
		{
			IList<UnitWrapper> unitWrappers = new List<UnitWrapper>(unitKeys.Count);

			foreach (UnitKey unitKey in unitKeys)
			{
				UnitWrapper unitWrapper = prefabFactory.GetUnitWrapperPrefab(unitKey);
				unitWrappers.Add(unitWrapper);
			}

			return unitWrappers;
		}

		// FELIX  Don't hardcode.  Database?
		private IList<BuildingKey> GetBuildOrder()
		{
			IList<BuildingKey> buildOrder = new List<BuildingKey>();

			buildOrder.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Factory, "NavalFactory"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"));
//			buildOrder.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));
			buildOrder.Add(new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"));

			return buildOrder;
		}

		void Update()
		{
			// FELIX  Adapt for IPad :P
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				modalMenuController.ShowMenu(OnModalMenuDismissed);
				PauseGame();
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
					Quit();
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

		private void Quit()
		{
			SceneManager.LoadScene(SceneNames.CHOOSE_LEVEL_SCENE);
		}
	}
}

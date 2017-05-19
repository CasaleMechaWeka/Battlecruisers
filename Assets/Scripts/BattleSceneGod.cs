using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.Fetchers;
using BattleCruisers.Fetchers.PrefabKeys;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI;
using BattleCruisers.UI.BuildMenus;
using BattleCruisers.UI.BuildingDetails;
using BattleCruisers.Utils;
using BattleCruisers.Units.Aircraft.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace BattleCruisers
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
		
		// User needs to be able to build at least one building
		private const int MIN_NUM_OF_BUILDING_GROUPS = 1;
		// Currently only support 6 types of buildings, so the UI is optimsed for this.  Ie, there is no space for more!
		private const int MAX_NUM_OF_BUILDING_GROUPS = 6;

		private const int CRUISER_OFFSET_IN_M = 35;

		void Awake()
		{
			Assert.raiseExceptions = true;


			// FELIX  Also inject and use ai loadout  (ie, allows AI and player to have different buildables, hulls)
			// FELIX  Inject
			Loadout playerLoadout = CreateLoadout();
			// FELIX  Get hull key from loadout
			HullKey playerHullKey = new HullKey("Trident");
			HullKey aiHullKey = new HullKey("Trident");


			// Common setup
			_buildingGroupFactory = new BuildingGroupFactory();
			
			PrefabFetcher prefabFetcher = new PrefabFetcher();
			prefabFactory.Initialise(prefabFetcher);


			// Player cruiser
			Cruiser playerCruiserPrefab = prefabFactory.GetCruiserPrefab(playerHullKey);
			_playerCruiser = prefabFactory.CreateCruiser(playerCruiserPrefab);
			_playerCruiser.transform.position = new Vector3(-CRUISER_OFFSET_IN_M, 0, 0);

			ITargetsFactory playerCruiserTargetsFactory = new TargetsFactory(_aiCruiser);
			IAircraftProvider playerCruiserAircraftProvider = new AircraftProvider(_playerCruiser.transform.position, _aiCruiser.transform.position);
			IDroneManager droneManager = new DroneManager();
			IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
			_playerCruiser.Initialise(droneManager, droneConsumerProvider, playerCruiserTargetsFactory, playerCruiserAircraftProvider, prefabFactory, Direction.Right);


			// UI
			ISpriteFetcher spriteFetcher = new SpriteFetcher();
			buildableDetailsController.Initialise(droneManager, spriteFetcher);
			uiFactory.Initialise(spriteFetcher, droneManager);


			IDictionary<BuildingCategory, IList<BuildingWrapper>> buildings = GetBuildingsFromKeys(playerLoadout, _playerCruiser, _aiCruiser);
			IList<BuildingGroup> buildingGroups = CreateBuildingGroups(buildings);
			IDictionary<UnitCategory, IList<UnitWrapper>> units = GetUnitsFromKeys(playerLoadout, _playerCruiser, _aiCruiser);
			buildMenuController.Initialise(buildingGroups, units);


			// AI cruiser
			Cruiser aiCruiserPrefab = prefabFactory.GetCruiserPrefab(aiHullKey);
			_aiCruiser = prefabFactory.CreateCruiser(aiCruiserPrefab);

			_aiCruiser.transform.position = new Vector3(CRUISER_OFFSET_IN_M, 0, 0);
			Quaternion rotation = _aiCruiser.transform.rotation;
			rotation.eulerAngles = new Vector3(0, 180, 0);
			_aiCruiser.transform.rotation = rotation;

			ITargetsFactory aiCruiserTargetsFactory = new TargetsFactory(_playerCruiser);
			IAircraftProvider aiCruiserAircraftProvider = new AircraftProvider(_aiCruiser.transform.position, _playerCruiser.transform.position);
			IDroneManager aiDroneManager = new DroneManager();
			IDroneConsumerProvider aiDroneConsumerProvider = new DroneConsumerProvider(aiDroneManager);
			_aiCruiser.Initialise(aiDroneManager, aiDroneConsumerProvider, aiCruiserTargetsFactory, aiCruiserAircraftProvider, prefabFactory, Direction.Left);


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

		// FELIX  Should not be hardcoded.  User loadouts should be in db?
		private Loadout CreateLoadout()
		{
			// Factories
			IList<BuildingKey> factories = new List<BuildingKey>();
			factories.Add(new BuildingKey(BuildingCategory.Factory, "AirFactory"));
			factories.Add(new BuildingKey(BuildingCategory.Factory, "NavalFactory"));
			factories.Add(new BuildingKey(BuildingCategory.Factory, "EngineeringBay"));
			
			// Tactical
			IList<BuildingKey> tactical = new List<BuildingKey>();
			tactical.Add(new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator"));

			// Defence
			IList<BuildingKey> defence = new List<BuildingKey>();
			defence.Add(new BuildingKey(BuildingCategory.Defence, "AntiShipTurret"));
			defence.Add(new BuildingKey(BuildingCategory.Defence, "AntiAirTurret"));

			// Offence
			IList<BuildingKey> offence = new List<BuildingKey>();
			offence.Add(new BuildingKey(BuildingCategory.Offence, "Artillery"));

			// Support
			IList<BuildingKey> support = new List<BuildingKey>();

			// Aircraft
			IList<UnitKey> aircraft = new List<UnitKey>();
			aircraft.Add(new UnitKey(UnitCategory.Aircraft, "Bomber"));
			aircraft.Add(new UnitKey(UnitCategory.Aircraft, "Fighter"));

			// Ships
			IList<UnitKey> ships = new List<UnitKey>();
			ships.Add(new UnitKey(UnitCategory.Naval, "AttackBoat"));
			ships.Add(new UnitKey(UnitCategory.Naval, "AttackBoat2"));

			return new Loadout(
				factories,
				tactical,
				defence,
				offence,
				support,
				aircraft,
				ships);
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

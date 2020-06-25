using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityDebug = UnityEngine.Debug;

namespace BattleCruisers.Utils
{
    // Do not change enum value order, as int values are used for comparison!
    public enum LoggingLevel
	{
		Warning, Normal, Verbose
	}

	public static class Tags
	{
		// Units
		public const string AIRCRAFT = "Aircraft";
		public const string FIGHTER = "Figher";
		public const string SHIPS = "Ships";

		// Buildings
		public const string BUILDABLE = "Buildable";
		public const string BUILDING = "Building";
		public const string DEFENSIVE_TURRET = "DefensiveTurret";
		public const string FACTORY = "Factory";

		// Projectiles
		public const string ACCURACY_ADJUSTERS = "AccuraryAdjusters";
        public const string ANGLE_CALCULATORS = "AngleCalculators";
        public const string BARREL_CONTROLLER = "BarrelController";
        public const string BARREL_WRAPPER = "BarrelWrapper";
        public const string BEAM = "Beam";
        public const string CLOSEST_POSITION_FINDER = "ClosestPositionFinder";
        public const string PROJECTILE_SPAWNER = "ProjectileSpawner";
        public const string SHELLS = "Shells";

		// Targets
		public const string RANKED_TARGET_TRACKER = "RankedTargetTracker";
		public const string TARGET = "Target";
		public const string TARGET_DETECTOR = "TargetDetector";
		public const string MANUAL_TARGET_DETECTOR = "ManualProximityTargetDetector";
		public const string TARGET_FINDER = "TargetFinder";
		public const string TARGET_FILTER = "TargetFilter";
        public const string TARGET_PROCESSORS = "TargetProcessors";
        public const string TARGET_PROVIDERS = "TargetProviders";
		public const string TARGET_RANGE_HELPER = "TargetRangeHelpers";
		public const string TARGET_TRACKER = "TargetTracker";
		public const string USER_CHOSEN_TARGET = "UserChosenTarget";

        // UI
		public const string LOADOUT_SCREEN = "LoadoutScreen";
        public const string MASKS = "Masks";
        public const string PREFAB_KEY_HELPER = "StaticPrefabKeyHelper";
        public const string PROGRESS_BARS = "ProgressBars";
        public const string SCREENS_SCENE_GOD = "ScreensSceneGod";
        public const string TUTORIAL_EXPLANATION_PANEL = "TutoraliExplanationPanel";
        public const string UI = "UI";
		public const string UI_MANAGER = "UIManager";

        // AI
        public const string AI = "AI";
		public const string AI_BUILD_ORDERS = "AIBuildOrders";
		public const string AI_TASKS = "Tasks";
        public const string DRONE_CONUMSER_FOCUS_MANAGER = "DroneConsumerFocusManager";

        // Drones
        public const string DRONE_FEEDBACK = "DroneFeedback";
        public const string DRONE_MANAGER = "DroneManager";
		public const string DRONE_CONSUMER_PROVIDER = "DroneConsumerProvider";

        // Movement
        public const string MOVEMENT = "Movement";
        public const string ROTATION_HELPER = "RotationHelper";
        public const string ROTATION_MOVEMENT_CONTROLLER = "RotationMovementController";
        public const string SHIP_MOVEMENT_DECIDER = "ShipMovementDecider";

        // Camera
        public const string CAMERA = "Camera";
        public const string CAMERA_CALCULATOR = "CameraCalculator";
        public const string CAMERA_FOCUSER = "CameraFocuser";
        public const string CAMERA_NAVIGATION_WHEEL_CALCULATOR = "CameraNavigationWheelCalculator";
        public const string CAMERA_TARGET_PROVIDER = "CameraTargetProvider";
        public const string DIRECTIONAL_ZOOM = "DirectionalZoom";
        public const string PINCH = "Pinch";
        public const string SCROLL_WHEEL_NAVIGATION = "ScrollWheelNavigation";
        public const string SWIPE_NAVIGATION = "SwipeNavigation";

        // Cruiser
        public const string CRUISER = "Cruiser";
        public const string UNIT_TARGETS = "UnitTargets";

        // Effects
        public const string CLOUDS = "Clouds";
        public const string DEATHS = "Deaths";
        public const string EXPLOSIONS = "Explosions";

        // Other
        public const string ALWAYS = "Always";
        public const string BATTLE_SCENE = "BattleScene";
        public const string BOOST = "Boost";
        public const string GENERIC = "Generic";
        public const string LIFETIME_EVENTS = "LifetimeEvents";
        public const string LOCAL_BOOSTER = "LocalBooster";
        public const string MODELS = "Model";
        public const string POOLS = "Pools";
        public const string PREDICTORS = "TargetPositionPredictors";
        public const string PREFAB_FACTORY = "PrefabFactory";
        public const string PYRAMID = "Pyramid";
        public const string REPAIR_MANAGER = "RepairManager";
        public const string SCENE_NAVIGATION = "SceneNavigation";
        public const string SLOTS = "Slots";
        public const string SOUND = "Sound";
        public const string TIME = "Time";
    }
	
	public static class Logging
	{
        //private const bool LOG_ALL = true;
        private const bool LOG_ALL = false;
        //private const LoggingLevel LOG_LEVEL = LoggingLevel.Normal;
        private const LoggingLevel LOG_LEVEL = LoggingLevel.Verbose;

        private static Dictionary<string, bool> _tagsToActiveness;
		private static Dictionary<string, bool> TagsToActiveness
		{
			get
			{
				if (_tagsToActiveness == null)
				{
					_tagsToActiveness = CreateDictionary();
				}
				return _tagsToActiveness;
			}
		}

		private static Dictionary<string, bool> CreateDictionary()
		{
			Dictionary<string, bool> tagsToActiveness = new Dictionary<string, bool>();

            // Units
            tagsToActiveness.Add(Tags.AIRCRAFT, false);
            tagsToActiveness.Add(Tags.FIGHTER, false);
            tagsToActiveness.Add(Tags.SHIPS, false);

            // Buildings
            tagsToActiveness.Add(Tags.BUILDABLE, false);
            tagsToActiveness.Add(Tags.BUILDING, false);
            tagsToActiveness.Add(Tags.DEFENSIVE_TURRET, false);
            tagsToActiveness.Add(Tags.FACTORY, false);

            // Projectiles
            tagsToActiveness.Add(Tags.ACCURACY_ADJUSTERS, false);
			tagsToActiveness.Add(Tags.ANGLE_CALCULATORS, false);
            tagsToActiveness.Add(Tags.BARREL_CONTROLLER, false);
            tagsToActiveness.Add(Tags.BARREL_WRAPPER, false);
            tagsToActiveness.Add(Tags.CLOSEST_POSITION_FINDER, false);
            tagsToActiveness.Add(Tags.BEAM, false);
            tagsToActiveness.Add(Tags.PROJECTILE_SPAWNER, false);
            tagsToActiveness.Add(Tags.SHELLS, false);

            // Targets
            tagsToActiveness.Add(Tags.RANKED_TARGET_TRACKER, false);
            tagsToActiveness.Add(Tags.TARGET, false);
            tagsToActiveness.Add(Tags.TARGET_DETECTOR, false);
            tagsToActiveness.Add(Tags.MANUAL_TARGET_DETECTOR, false);
            tagsToActiveness.Add(Tags.TARGET_FINDER, false);
            tagsToActiveness.Add(Tags.TARGET_FILTER, false);
            tagsToActiveness.Add(Tags.TARGET_PROCESSORS, false);
            tagsToActiveness.Add(Tags.TARGET_PROVIDERS, false);
            tagsToActiveness.Add(Tags.TARGET_RANGE_HELPER, false);
            tagsToActiveness.Add(Tags.TARGET_TRACKER, false);
            tagsToActiveness.Add(Tags.USER_CHOSEN_TARGET, false);

            // UI
            tagsToActiveness.Add(Tags.LOADOUT_SCREEN, false);
            tagsToActiveness.Add(Tags.MASKS, false);
            tagsToActiveness.Add(Tags.PREFAB_KEY_HELPER, false);
            tagsToActiveness.Add(Tags.PROGRESS_BARS, false);
            tagsToActiveness.Add(Tags.SCREENS_SCENE_GOD, false);
            tagsToActiveness.Add(Tags.TUTORIAL_EXPLANATION_PANEL, false);
            tagsToActiveness.Add(Tags.UI_MANAGER, false);
            tagsToActiveness.Add(Tags.UI, false);

            // AI
            tagsToActiveness.Add(Tags.AI, false);
            tagsToActiveness.Add(Tags.AI_BUILD_ORDERS, false);
            tagsToActiveness.Add(Tags.AI_TASKS, false);
            tagsToActiveness.Add(Tags.DRONE_CONUMSER_FOCUS_MANAGER, false);

            // Drones
            tagsToActiveness.Add(Tags.DRONE_FEEDBACK, false);
            tagsToActiveness.Add(Tags.DRONE_MANAGER, false);
            tagsToActiveness.Add(Tags.DRONE_CONSUMER_PROVIDER, false);

            // Movement
            tagsToActiveness.Add(Tags.MOVEMENT, false);
            tagsToActiveness.Add(Tags.ROTATION_HELPER, false);
            tagsToActiveness.Add(Tags.ROTATION_MOVEMENT_CONTROLLER, false);
            tagsToActiveness.Add(Tags.SHIP_MOVEMENT_DECIDER, false);

            // Camera
            tagsToActiveness.Add(Tags.CAMERA, false);
            tagsToActiveness.Add(Tags.CAMERA_CALCULATOR, false);
            tagsToActiveness.Add(Tags.CAMERA_FOCUSER, false);
            tagsToActiveness.Add(Tags.CAMERA_NAVIGATION_WHEEL_CALCULATOR, false);
            tagsToActiveness.Add(Tags.CAMERA_TARGET_PROVIDER, false);
            tagsToActiveness.Add(Tags.DIRECTIONAL_ZOOM, false);
            tagsToActiveness.Add(Tags.PINCH, false);
            tagsToActiveness.Add(Tags.SCROLL_WHEEL_NAVIGATION, false);
            tagsToActiveness.Add(Tags.SWIPE_NAVIGATION, false);

            // Cruiser
            tagsToActiveness.Add(Tags.CRUISER, false);
            tagsToActiveness.Add(Tags.UNIT_TARGETS, false);

            // Effects
            tagsToActiveness.Add(Tags.CLOUDS, false);
            tagsToActiveness.Add(Tags.DEATHS, false);
            tagsToActiveness.Add(Tags.EXPLOSIONS, false);

            // Other
            tagsToActiveness.Add(Tags.ALWAYS, true);
            tagsToActiveness.Add(Tags.BOOST, false);
            tagsToActiveness.Add(Tags.BATTLE_SCENE, false);
            tagsToActiveness.Add(Tags.GENERIC, true);
            tagsToActiveness.Add(Tags.LIFETIME_EVENTS, false);
            tagsToActiveness.Add(Tags.LOCAL_BOOSTER, false);
            tagsToActiveness.Add(Tags.MODELS, false);
            tagsToActiveness.Add(Tags.POOLS, false);
            tagsToActiveness.Add(Tags.PREDICTORS, false);
            tagsToActiveness.Add(Tags.PREFAB_FACTORY, false);
            tagsToActiveness.Add(Tags.PYRAMID, false);
            tagsToActiveness.Add(Tags.REPAIR_MANAGER, false);
            tagsToActiveness.Add(Tags.SCENE_NAVIGATION, false);
            tagsToActiveness.Add(Tags.SLOTS, false);
            tagsToActiveness.Add(Tags.SOUND, false);
            tagsToActiveness.Add(Tags.TIME, false);

            return tagsToActiveness;
		}

        [Conditional("ENABLE_LOGS")]
        public static void LogMethod(
            string tag,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(LoggingLevel.Normal, tag, string.Empty, memberName, sourceFilePath, sourceLineNumber);
        }

        [Conditional("ENABLE_LOGS")]
        public static void Log(
            string tag,
            object obj,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(LoggingLevel.Normal, tag, obj?.ToString(), memberName, sourceFilePath, sourceLineNumber);
        }

        [Conditional("ENABLE_LOGS")]
        public static void Log(
            string tag, 
            string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Log(LoggingLevel.Normal, tag, message, memberName, sourceFilePath, sourceLineNumber);
        }

        [Conditional("ENABLE_LOGS")]
        public static void VerboseMethod(
            string tag,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(LoggingLevel.Verbose, tag, string.Empty, memberName, sourceFilePath, sourceLineNumber);
        }

        [Conditional("ENABLE_LOGS")]
		public static void Verbose(
            string tag, 
            string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(LoggingLevel.Verbose, tag, message, memberName, sourceFilePath, sourceLineNumber);
        }

        [Conditional("ENABLE_LOGS")]
        public static void Warn(
            string tag, 
            string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
		{
			Log(LoggingLevel.Warning, tag, message, memberName, sourceFilePath, sourceLineNumber);
        }

		private static void Log(LoggingLevel logLevel, string tag, string message, string memberName, string sourceFilePath, int sourceLineNumber)
		{
			if (LOG_LEVEL >= logLevel
				&& (LOG_ALL || TagsToActiveness[tag]))
			{
                string fullMessage = CreateMessage(tag, message, memberName, sourceFilePath, sourceLineNumber);

				if (logLevel == LoggingLevel.Warning)
				{
					UnityDebug.LogWarning(fullMessage);
				}
				else
				{
					UnityDebug.Log(fullMessage);
				}
			}
		}

        // This method is extremely expensive in terms of memory.  Hence only call when logging
        // is enabled (in debug builds).
        private static string CreateMessage(string tag, string baseMessage, string memberName, string sourceFilePath, int sourceLineNumber)
        {
			string timestamp = DateTime.Now.ToString("hh:mm:ss.fff");
            string fileName = sourceFilePath.Split('\\').Last();
            return $"{timestamp} - {tag}:  {fileName}:{memberName}[{sourceLineNumber}]: {baseMessage}";
        }
	}
}

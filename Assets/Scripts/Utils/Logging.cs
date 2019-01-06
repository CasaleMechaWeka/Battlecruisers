using System;
using System.Collections.Generic;
using UnityEngine;

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
        public const string SHELL_SPAWNER = "ShellSpawner";
        public const string SHELLS = "Shells";

		// Targets
		public const string TARGET = "Target";
		public const string TARGET_DETECTOR = "TargetDetector";
		public const string TARGET_FINDER = "TargetFinder";
		public const string TARGET_FILTER = "TargetFilter";
        public const string TARGET_PROCESSORS = "TargetProcessors";
        public const string TARGET_PROVIDERS = "TargetProviders";
		public const string TARGET_RANGE_HELPER = "TargetRangeHelpers";
		public const string TARGET_TRACKER = "TargetTracker";
		public const string RANKED_TARGET_TRACKER = "RankedTargetTracker";

        // UI
        public const string PROGRESS_BARS = "ProgressBars";
		public const string UI_MANAGER = "UIManager";

        // AI
		public const string AI = "AI";
		public const string AI_BUILD_ORDERS = "AIBuildOrders";
		public const string AI_TASKS = "Tasks";
        public const string DRONE_CONUMSER_FOCUS_MANAGER = "DroneConsumerFocusManager";

        // Drones
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

        // Other
        public const string CRUISER = "Cruiser";
		public const string GENERIC = "Generic";
        public const string LOCAL_BOOSTER = "LocalBooster";
        public const string PREDICTORS = "TargetPositionPredictors";
        public const string REPAIR_MANAGER = "RepairManager";
        public const string SCENE_NAVIGATION = "SceneNavigation";
        public const string SLOTS = "Slots";
    }
	
	public static class Logging
	{
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
            tagsToActiveness.Add(Tags.SHELL_SPAWNER, false);
			tagsToActiveness.Add(Tags.SHELLS, false);

            // Targets
            tagsToActiveness.Add(Tags.TARGET, false);
            tagsToActiveness.Add(Tags.TARGET_DETECTOR, false);
            tagsToActiveness.Add(Tags.TARGET_FINDER, false);
            tagsToActiveness.Add(Tags.TARGET_FILTER, false);
            tagsToActiveness.Add(Tags.TARGET_PROCESSORS, false);
            tagsToActiveness.Add(Tags.TARGET_PROVIDERS, false);
            tagsToActiveness.Add(Tags.TARGET_RANGE_HELPER, false);
            tagsToActiveness.Add(Tags.TARGET_TRACKER, false);
            tagsToActiveness.Add(Tags.RANKED_TARGET_TRACKER, false);

            // UI
            tagsToActiveness.Add(Tags.PROGRESS_BARS, false);
            tagsToActiveness.Add(Tags.UI_MANAGER, false);

            // AI
            tagsToActiveness.Add(Tags.AI, false);
            // FELIX
            tagsToActiveness.Add(Tags.AI_BUILD_ORDERS, true);
            //tagsToActiveness.Add(Tags.AI_BUILD_ORDERS, false);
            tagsToActiveness.Add(Tags.AI_TASKS, false);
            tagsToActiveness.Add(Tags.DRONE_CONUMSER_FOCUS_MANAGER, false);

            // Drones
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

            // Other
            tagsToActiveness.Add(Tags.CRUISER, false);
            tagsToActiveness.Add(Tags.GENERIC, true);
            tagsToActiveness.Add(Tags.LOCAL_BOOSTER, false);
            tagsToActiveness.Add(Tags.PREDICTORS, false);
            tagsToActiveness.Add(Tags.REPAIR_MANAGER, false);
            tagsToActiveness.Add(Tags.SCENE_NAVIGATION, false);
            tagsToActiveness.Add(Tags.SLOTS, false);

            return tagsToActiveness;
		}

        public static void Log(string message)
        {
            Log(Tags.GENERIC, message);
        }

		public static void Log(string tag, string message)
		{
			Log(LoggingLevel.Normal, tag, message);
		}

		public static void Log(string tag, object obj, string message)
		{
			Log(tag, GetClassName(obj) + "." + message);
		}

		private static string GetClassName(object obj)
		{
			string[] fullyQualifiedName = obj.GetType().ToString().Split('.');
			return fullyQualifiedName[fullyQualifiedName.Length - 1];
		}

		public static void Verbose(string tag, string message)
		{
			Log(LoggingLevel.Verbose, tag, message);
		}

		public static void Warn(string tag, string message)
		{
			Log(LoggingLevel.Warning, tag, message);
		}

        public static void Log<T>(string tag, IList<T> items)
        {
            for (int i = 0; i < items.Count; ++i)
            {
                Log(tag, i + " " + items[i]);
            }
        }

		private static void Log(LoggingLevel logLevel, string tag, string message)
		{
			if (LOG_LEVEL >= logLevel
				&& (TagsToActiveness[tag] || LOG_ALL))
			{
				string timestamp = DateTime.Now.ToString("hh:mm:ss.fff");
				string fullMsg = timestamp + "-" + tag + ":  " + message;

				if (logLevel == LoggingLevel.Warning)
				{
					Debug.LogWarning(fullMsg);
				}
				else
				{
					Debug.Log(fullMsg);
				}
			}
		}
	}
}

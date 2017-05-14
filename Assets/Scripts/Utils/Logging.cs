using System;
using System.Collections;
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
		public const string ATTACK_BOAT = "AttackBoat";

		// Buildings
		public const string DEFENSIVE_TURRET = "DefensiveTurret";
		public const string FACTORY = "Factory";

		// Projectiles
		public const string ANGLE_CALCULATORS = "AngleCalculators";
		public const string BARREL_CONTROLLER = "BarrelController";
		public const string SHELL_SPAWNER = "ShellSpawner";
		public const string SHELLS = "Shells";
		public const string TARGET = "Target";
		public const string TARGET_DETECTOR = "TargetDetector";
		public const string TARGET_PROCESSORS = "TargetProcessors";

		// UI
		public const string PROGRESS_BARS = "ProgressBars";
		public const string UI_MANAGER = "UIManager";

		// Other
		public const string AI = "AI";
		public const string CAMERA_CONTROLLER = "CameraController";
		public const string CRUISER = "Cruiser";
		public const string DRONES = "Drones";
	}
	
	public static class Logging
	{
		private const bool LOG_ALL = false;
		private const LoggingLevel LOG_LEVEL = LoggingLevel.Normal;
//		private const LoggingLevel LOG_LEVEL = LoggingLevel.Verbose;

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
			tagsToActiveness.Add(Tags.AIRCRAFT, true);
			tagsToActiveness.Add(Tags.ATTACK_BOAT, false);

			// Buildings
			tagsToActiveness.Add(Tags.DEFENSIVE_TURRET, false);
			tagsToActiveness.Add(Tags.FACTORY, false);

			// Projectiles
			tagsToActiveness.Add(Tags.BARREL_CONTROLLER, false);
			tagsToActiveness.Add(Tags.SHELL_SPAWNER, false);
			tagsToActiveness.Add(Tags.SHELLS, false);
			tagsToActiveness.Add(Tags.ANGLE_CALCULATORS, false);
			tagsToActiveness.Add(Tags.TARGET, false);
			tagsToActiveness.Add(Tags.TARGET_DETECTOR, false);
			tagsToActiveness.Add(Tags.TARGET_PROCESSORS, false);

			// UI
			tagsToActiveness.Add(Tags.PROGRESS_BARS, false);
			tagsToActiveness.Add(Tags.UI_MANAGER, false);

			// Other
			tagsToActiveness.Add(Tags.AI, false);
			tagsToActiveness.Add(Tags.CAMERA_CONTROLLER, false);
			tagsToActiveness.Add(Tags.CRUISER, false);
			tagsToActiveness.Add(Tags.DRONES, false);

			return tagsToActiveness;
		}

		public static void Log(string tag, string message)
		{
			Log(LoggingLevel.Normal, tag, message);
		}

		public static void Log(string tag, object obj, string message)
		{
			Log(tag, $"{GetClassName(obj)}.{message}");
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

		private static void Log(LoggingLevel logLevel, string tag, string message)
		{
			if (LOG_LEVEL >= logLevel
				&& (TagsToActiveness[tag] || LOG_ALL))
			{
				string timestamp = DateTime.Now.ToString("hh:mm:ss.fff");
				string fullMsg = $"{timestamp}-{tag}:  {message}";

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils
{
	public enum LoggingLevel
	{
		Normal, Verbose
	}

	public static class Tags
	{
		public const string DRONES = "Drones";
		public const string FACTORY = "Factory";
		public const string AI = "AI";
		public const string ATTACK_BOAT = "AttackBoat";
		public const string DEFENSIVE_TURRET = "DefensiveTurret";
		public const string TURRET_BARREL_CONTROLLER = "TurretBarrelController";
		public const string CAMERA_CONTROLLER = "CameraController";
		public const string SHELL_SPAWNER = "ShellSpawner";
		public const string SHELLS = "Shells";
		public const string BOMBER = "Bomber";
		public const string UI_MANAGER = "UIManager";
		public const string TARGET = "Target";
		public const string PROGRESS_BARS = "ProgressBars";
		public const string ANGLE_CALCULATORS = "AngleCalculators";
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

			tagsToActiveness.Add(Tags.DRONES, false);
			tagsToActiveness.Add(Tags.FACTORY, false);
			tagsToActiveness.Add(Tags.AI, false);
			tagsToActiveness.Add(Tags.ATTACK_BOAT, false);
			tagsToActiveness.Add(Tags.DEFENSIVE_TURRET, false);
			tagsToActiveness.Add(Tags.TURRET_BARREL_CONTROLLER, false);
			tagsToActiveness.Add(Tags.SHELL_SPAWNER, false);
			tagsToActiveness.Add(Tags.CAMERA_CONTROLLER, false);
			tagsToActiveness.Add(Tags.SHELLS, false);
			tagsToActiveness.Add(Tags.BOMBER, false);
			tagsToActiveness.Add(Tags.UI_MANAGER, false);
			tagsToActiveness.Add(Tags.TARGET, false);
			tagsToActiveness.Add(Tags.PROGRESS_BARS, false);
			tagsToActiveness.Add(Tags.ANGLE_CALCULATORS, false);

			return tagsToActiveness;
		}

		public static void Log(string tag, string message)
		{
			if (TagsToActiveness[tag] || LOG_ALL)
			{
				Debug.Log(tag + ":  " + message);
			}
		}

		public static void Verbose(string tag, string message)
		{
			#pragma warning disable 162
			if (LOG_LEVEL >= LoggingLevel.Verbose)
			{
				Log(tag, message);
			}
			#pragma warning restore 162
		}
	}
}

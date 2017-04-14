using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils
{
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
	}
	
	public static class Logging
	{
		private static Dictionary<string, bool> _tagsToActiveness;

		public static void Initialise()
		{
			_tagsToActiveness = new Dictionary<string, bool>();
			_tagsToActiveness.Add(Tags.DRONES, false);
			_tagsToActiveness.Add(Tags.FACTORY, false);
			_tagsToActiveness.Add(Tags.AI, false);
			_tagsToActiveness.Add(Tags.ATTACK_BOAT, false);
			_tagsToActiveness.Add(Tags.DEFENSIVE_TURRET, false);
			_tagsToActiveness.Add(Tags.TURRET_BARREL_CONTROLLER, true);
			_tagsToActiveness.Add(Tags.SHELL_SPAWNER, true);
			_tagsToActiveness.Add(Tags.CAMERA_CONTROLLER, false);
		}

		public static void Log(string tag, string message)
		{
			if (_tagsToActiveness[tag])
			{
				Debug.Log(tag + ":  " + message);
			}
		}
	}
}

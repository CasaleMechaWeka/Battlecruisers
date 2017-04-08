using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Utils
{
	public static class Tags
	{
		public const string DRONES = "Drones";
		public const string FACTORY = "Factory";
	}
	
	public static class Logging
	{
		private static Dictionary<string, bool> _tagsToActiveness;

		public static void Initialise()
		{
			_tagsToActiveness = new Dictionary<string, bool>();
			_tagsToActiveness.Add(Tags.DRONES, true);
			_tagsToActiveness.Add(Tags.FACTORY, true);
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

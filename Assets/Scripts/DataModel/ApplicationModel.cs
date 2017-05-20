using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.DataModel
{
	public static class ApplicationModel
	{
		public static int SelectedLevel { get; set; }

		private static ILoadoutManager _loadoutManager;
		public static ILoadoutManager LoadoutManager
		{
			get
			{
				if (_loadoutManager == null)
				{
					_loadoutManager = new LoadoutManager();
				}
				return _loadoutManager;
			}
		}
	}
}

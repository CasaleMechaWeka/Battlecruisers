using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public class ScreenBC : IScreen
	{
		public float Width { get { return Screen.width; } }
		public float Height { get { return Screen.height; } }
	}
}

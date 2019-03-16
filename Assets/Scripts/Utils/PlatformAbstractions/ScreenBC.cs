using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public class ScreenBC : IScreen
	{
		public float Width => Screen.width;
		public float Height => Screen.height;
	}
}

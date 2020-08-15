using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public class ScreenBC : IScreen
	{
		public float WidthInPixels => Screen.width;
		public float HeightInPixels => Screen.height;
	}
}

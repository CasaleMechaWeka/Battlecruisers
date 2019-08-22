using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
	public class CameraToggleTestGod : TestGodBase
	{
		public Camera overviewCamera, closeUpCamera;

		protected override void Start()
		{
            base.Start();

			closeUpCamera.enabled = true;
			overviewCamera.enabled = false;
		}

		public void ToggleCamera()
		{
			overviewCamera.enabled = !overviewCamera.enabled;
			closeUpCamera.enabled = !closeUpCamera.enabled;
		}
	}
}

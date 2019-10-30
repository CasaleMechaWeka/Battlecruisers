using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class CameraToggleTestGod : TestGodBase
	{
		public Camera overviewCamera, closeUpCamera;

        protected override void Setup(Helper helper)
        {
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

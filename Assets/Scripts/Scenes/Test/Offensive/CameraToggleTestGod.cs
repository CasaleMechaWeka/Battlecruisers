using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class CameraToggleTestGod : TestGodBase
	{
		public Camera overviewCamera, closeUpCamera;
		public bool showCloseupFirst = true;

        protected override void Setup(Helper helper)
        {
			closeUpCamera.enabled = showCloseupFirst;
			overviewCamera.enabled = !showCloseupFirst;
        }

        public void ToggleCamera()
		{
			overviewCamera.enabled = !overviewCamera.enabled;
			closeUpCamera.enabled = !closeUpCamera.enabled;
		}
	}
}

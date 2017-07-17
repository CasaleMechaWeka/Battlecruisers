using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
	public class CameraToggleTestGod : MonoBehaviour 
	{
		public Camera overviewCamera, closeUpCamera;

		void Start()
		{
			closeUpCamera.enabled = true;
			overviewCamera.enabled = false;

			OnStart();
		}

		protected virtual void OnStart() { }

		public void ToggleCamera()
		{
			overviewCamera.enabled = !overviewCamera.enabled;
			closeUpCamera.enabled = !closeUpCamera.enabled;
		}
	}
}

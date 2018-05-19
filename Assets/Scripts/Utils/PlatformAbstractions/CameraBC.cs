using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public class CameraBC : ICamera
    {
		private readonly Camera _platfromCamera;

		public CameraBC(Camera platformCamera)
		{
			Assert.IsNotNull(platformCamera);
			_platfromCamera = platformCamera;
		}

		public Vector3 Position 
		{ 
			get { return _platfromCamera.transform.position; }
			set { _platfromCamera.transform.position = value; }
		}

		public float OrthographicSize
		{
			get { return _platfromCamera.orthographicSize; }
			set { _platfromCamera.orthographicSize = value; }
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions
{
	public class CameraBC : ICamera
    {
		private readonly Camera _platfromCamera;

        public event EventHandler OrthographicSizeChanged;

        public Vector3 Position 
		{ 
			get { return _platfromCamera.transform.position; }
			set { _platfromCamera.transform.position = value; }
		}

		public float OrthographicSize
		{
			get { return _platfromCamera.orthographicSize; }
			set
            {
                _platfromCamera.orthographicSize = value;

                OrthographicSizeChanged?.Invoke(this, EventArgs.Empty);
            }
		}

        public float Aspect => _platfromCamera.aspect;
        public float PixelWidth => _platfromCamera.pixelWidth;
        public float PixelHeight => _platfromCamera.pixelHeight;
        public ITransform Transform { get; }

        public CameraBC(Camera platformCamera)
		{
			Assert.IsNotNull(platformCamera);
			_platfromCamera = platformCamera;

            Transform = new TransformBC(_platfromCamera.transform);
		}

        // Not a getter as the value needs to be calculated.  Ie, not a straight
        // proxy to the platform object.
        public Vector2 GetSize()
        {
            float height = _platfromCamera.orthographicSize * 2;
            float width = _platfromCamera.aspect * height;
            return new Vector2(width, height);
        }

        public Vector3 WorldToViewportPoint(Vector3 worldPoint)
        {
            return _platfromCamera.WorldToViewportPoint(worldPoint);
        }

        public Vector3 WorldToScreenPoint(Vector3 worldPoint)
        {
            return _platfromCamera.WorldToScreenPoint(worldPoint);
        }

        public Vector3 ScreenToWorldPoint(Vector3 screenPoint)
        {
            return _platfromCamera.ScreenToWorldPoint(screenPoint);
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class CameraBC : ICamera
    {
		private readonly Camera _platformCamera;

        public event EventHandler OrthographicSizeChanged;
        public event EventHandler PositionChanged;

        public Vector3 Position 
		{ 
			get { return _platformCamera.transform.position; }
			set 
            { 
                _platformCamera.transform.position = value;
                PositionChanged?.Invoke(this, EventArgs.Empty);
            }
		}

		public float OrthographicSize
		{
			get { return _platformCamera.orthographicSize; }
			set
            {
                _platformCamera.orthographicSize = value;
                OrthographicSizeChanged?.Invoke(this, EventArgs.Empty);
            }
		}

        public float Aspect => _platformCamera.aspect;
        public float PixelWidth => _platformCamera.pixelWidth;
        public float PixelHeight => _platformCamera.pixelHeight;
        
        public float FieldOfView
        {
            get => _platformCamera.fieldOfView;
            set => _platformCamera.fieldOfView = value;
        }

        public CameraBC(Camera platformCamera)
		{
			Assert.IsNotNull(platformCamera);
			_platformCamera = platformCamera;
		}

        // Not a getter as the value needs to be calculated.  Ie, not a straight
        // proxy to the platform object.
        public Vector2 GetSize()
        {
            float height = _platformCamera.orthographicSize * 2;
            float width = _platformCamera.aspect * height;
            return new Vector2(width, height);
        }

        public Vector3 WorldToViewportPoint(Vector3 worldPoint)
        {
            return _platformCamera.WorldToViewportPoint(worldPoint);
        }

        public Vector3 WorldToScreenPoint(Vector3 worldPoint)
        {
            return _platformCamera.WorldToScreenPoint(worldPoint);
        }

        public Vector3 ScreenToWorldPoint(Vector3 screenPoint)
        {
            return _platformCamera.ScreenToWorldPoint(screenPoint);
        }
    }
}

using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
	/// <summary>
	/// Zooms in and out when the mouse wheel is scrolled.  Similar to Forged
	/// Alliance scroll, except it is not directional (ie, the mouse position is
	/// irrelevant, we will not zoom towards the mouse position).
	/// </summary>
	public class MouseZoomHandler : IMouseZoomHandler
	{
        private readonly ICamera _camera;
		private readonly ISettingsManager _settingsManager;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
        private readonly ICameraCalculator _calculator;
        private readonly IPositionClamper _cameraPositionClamper;
        private readonly float _minOrthographicSize;
		private readonly float _maxOrthographicSize;

		// Originally did not take time delta into consideration.  So multiply
		// by this constant so zoom is roughly the same when time delta is normal.
		private const float ZOOM_SPEED_MULTIPLIER = 30;

        public MouseZoomHandler(
            ICamera camera,
            ISettingsManager settingsManager, 
            IDeltaTimeProvider deltaTimeProvider,
            ICameraCalculator calculator, 
            IPositionClamper cameraPositionClamper,
            float minOrthographicSize, 
            float maxOrthographicSize)
		{
            Helper.AssertIsNotNull(camera, settingsManager, deltaTimeProvider, calculator, cameraPositionClamper);
			Assert.IsTrue(minOrthographicSize < maxOrthographicSize);

            _camera = camera;
			_settingsManager = settingsManager;
            _deltaTimeProvider = deltaTimeProvider;
            _calculator = calculator;
            _cameraPositionClamper = cameraPositionClamper;
			_minOrthographicSize = minOrthographicSize;
			_maxOrthographicSize = maxOrthographicSize;
		}

        public MouseZoomResult HandleZoom(Vector3 zoomTargetPosition, float yMouseScrollDelta)
        {
            float newOrthographicSize = FindCameraOrthographicSize(yMouseScrollDelta);

            Vector3 newCameraPosition = _camera.Position;

            if (yMouseScrollDelta > 0)
            {
                // Only zooming in is directional
                newCameraPosition = FindZoomingInCameraPosition(zoomTargetPosition, newOrthographicSize);
            }

            return new MouseZoomResult(newOrthographicSize, newCameraPosition);
        }

		private float FindCameraOrthographicSize(float yMouseScrollDelta)
		{
			if (!Mathf.Approximately(yMouseScrollDelta, 0))
			{
                _camera.OrthographicSize -= _settingsManager.ZoomSpeed * yMouseScrollDelta * ZOOM_SPEED_MULTIPLIER * _deltaTimeProvider.UnscaledDeltaTime;
				_camera.OrthographicSize = Mathf.Clamp(_camera.OrthographicSize, _minOrthographicSize, _maxOrthographicSize);
			}

			return _camera.OrthographicSize;
		}

        private Vector3 FindZoomingInCameraPosition(Vector3 zoomTargetPosition, float newOrthographicSize)
        {
            Vector3 targetViewportPosition = _camera.WorldToViewportPoint(zoomTargetPosition);

            return
                _calculator.FindZoomingCameraPosition(
                    zoomTargetPosition,
                    targetViewportPosition,
                    newOrthographicSize,
                    _camera.Aspect,
                    _camera.Position.z);
        }
    }
}

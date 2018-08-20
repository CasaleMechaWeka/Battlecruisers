using BattleCruisers.Data.Settings;
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
		private readonly ISettingsManager _settingsManager;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
		private readonly float _minOrthographicSize;
		private readonly float _maxOrthographicSize;

		// Originally did not take time delta into consideration.  So multiply
		// by this constant so zoom is roughly the same when time delta is normal.
		private const float ZOOM_SPEED_MULTIPLIER = 30;

        public MouseZoomHandler(ISettingsManager settingsManager, IDeltaTimeProvider deltaTimeProvider, float minOrthographicSize, float maxOrthographicSize)
		{
            Helper.AssertIsNotNull(settingsManager, deltaTimeProvider);
			Assert.IsTrue(minOrthographicSize < maxOrthographicSize);

			_settingsManager = settingsManager;
            _deltaTimeProvider = deltaTimeProvider;
			_minOrthographicSize = minOrthographicSize;
			_maxOrthographicSize = maxOrthographicSize;
		}

		public float FindCameraOrthographicSize(float cameraOrthographicSize, float yMouseScrollDelta)
		{
			if (!Mathf.Approximately(yMouseScrollDelta, 0))
			{
                cameraOrthographicSize -= _settingsManager.ZoomSpeed * yMouseScrollDelta * ZOOM_SPEED_MULTIPLIER * _deltaTimeProvider.UnscaledDeltaTime;
				cameraOrthographicSize = Mathf.Clamp(cameraOrthographicSize, _minOrthographicSize, _maxOrthographicSize);
			}

			return cameraOrthographicSize;
		}
	}
}

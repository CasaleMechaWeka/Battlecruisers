using BattleCruisers.Data.Settings;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
	/// <summary>
	/// Zooms in and out when the mouse wheel is scrolled.  Similar to Forged
	/// Alliance scroll, except it is not directional (ie, the mouse position is
	/// irrelevant, we will not zoom towards the mouse position).
	/// </summary>
	/// FELIX  Test :D
	public class MouseZoomHandler : IMouseZoomHandler
	{
		private readonly ISettingsManager _settingsManager;
		private readonly float _minOrthographicSize;
		private readonly float _maxOrthographicSize;

		public MouseZoomHandler(ISettingsManager settingsManager, float minOrthographicSize, float maxOrthographicSize)
		{
			Assert.IsNotNull(settingsManager);
			Assert.IsTrue(minOrthographicSize < maxOrthographicSize);

			_settingsManager = settingsManager;
			_minOrthographicSize = minOrthographicSize;
			_maxOrthographicSize = maxOrthographicSize;
		}

		public float FindCameraOrthographicSize(float cameraOrthographicSize, float yMouseScrollDelta, float timeDelta)
		{
			if (!Mathf.Approximately(yMouseScrollDelta, 0))
			{
				// FELIX  Take timeDelta into consideration :/
				cameraOrthographicSize -= _settingsManager.ZoomSpeed * yMouseScrollDelta;
				cameraOrthographicSize = Mathf.Clamp(cameraOrthographicSize, _minOrthographicSize, _maxOrthographicSize);
			}

			return cameraOrthographicSize;
		}
	}
}

using UnityEngine;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
	public interface IMouseZoomHandler
	{
		float FindCameraOrthographicSize(float cameraOrthographicSize, float yMouseScrollDelta, float timeDelta);
	}
}

using UnityEngine;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
	public interface IScrollHandler
	{
		Vector3 FindCameraPosition(float cameraOrthographicSize, Vector3 cameraPosition, Vector3 mousePosition);
	}
}

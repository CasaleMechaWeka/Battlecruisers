using System.Collections.Generic;

namespace BattleCruisers.UI.Cameras
{
	public interface ICameraTargetsFactory
	{
		IDictionary<CameraState, ICameraTarget> CreateCameraTargets();
	}
}

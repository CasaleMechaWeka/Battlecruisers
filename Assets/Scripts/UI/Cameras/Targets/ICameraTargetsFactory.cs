using System.Collections.Generic;

namespace BattleCruisers.UI.Cameras.Targets
{
	public interface ICameraTargetsFactory
	{
		IDictionary<CameraState, ICameraTargetLegacy> CreateCameraTargets();
	}
}

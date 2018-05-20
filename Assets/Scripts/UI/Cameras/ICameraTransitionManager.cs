namespace BattleCruisers.UI.Cameras
{
	public interface ICameraTransitionManager : ICameraMover
	{
		CameraState CameraTarget { set; }
	}
}

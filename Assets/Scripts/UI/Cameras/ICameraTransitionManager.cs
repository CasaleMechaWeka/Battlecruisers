namespace BattleCruisers.UI.Cameras
{
	public interface ICameraTransitionManager : ICameraMover
	{
		/// <returns>
		/// <c>true</c>, if camera was or will be moved, <c>false</c> otherwise.
		/// The camera may not be moved if it is currently in a transition, or
		/// the new target is the same as the current target.
		/// </returns>
		bool SetCameraTarget(CameraState targetState);
	}
}

namespace BattleCruisers.UI.Cameras
{
	// FELIX  Create movers namespace?  Put adjusers userinput namespaces in that?
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

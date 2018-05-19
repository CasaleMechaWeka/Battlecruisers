namespace BattleCruisers.UI.Cameras
{
	// FELIX  Create movers namespace?  Put adjusers userinput namespaces in that?
	public interface ICameraTransitionManager : ICameraMover
	{
		// FELIX  Expose getter too, so don't need to set if already in the target state?
		// FELIX  Convert to property
		void SetCameraTarget(CameraState targetState);
	}
}

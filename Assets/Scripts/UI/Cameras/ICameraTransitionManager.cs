namespace BattleCruisers.UI.Cameras
{
	// FELIX  Create movers namespace?  Put adjusers userinput namespaces in that?
	public interface ICameraTransitionManager : ICameraMover
	{
		CameraState TargetState { set; }
	}
}

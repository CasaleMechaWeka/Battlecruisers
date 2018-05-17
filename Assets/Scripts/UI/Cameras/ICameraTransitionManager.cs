namespace BattleCruisers.UI.Cameras
{
	public interface ICameraTransitionManager
	{
		CameraTarget Target { set; }

        /// <summary>
        /// Should be called every frame to smoothly move the camera to the 
		/// target position.
        /// </summary>
		void MoveCamera();
	}
}

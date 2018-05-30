using System;

namespace BattleCruisers.UI.Cameras
{
	public interface IUserInputCameraMover : ICameraMover
	{
		event EventHandler Zoomed;
		event EventHandler Scrolled;
	}
}

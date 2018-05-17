using UnityEngine;

namespace BattleCruisers.UI.Cameras.Adjusters
{
	public interface ISmoothPositionAdjuster
	{
		/// <returns>
        /// <c>true</c>, if the camera position is the same as the given 
        /// <paramref name="targetPosition"/>, <c>false</c> otherwise.
        /// </returns>
		bool AdjustPosition(Vector3 targetPosition);
	}
}

namespace BattleCruisers.UI.Cameras.Adjusters
{
	public interface ISmoothZoomAdjuster
	{
        /// <returns>
		/// <c>true</c>, if the zoom level is the same as the given 
		/// <paramref name="targetOrthographicSize"/>, <c>false</c> otherwise.
		/// </returns>
		bool AdjustZoom(float targetOrthographicSize);
	}
}

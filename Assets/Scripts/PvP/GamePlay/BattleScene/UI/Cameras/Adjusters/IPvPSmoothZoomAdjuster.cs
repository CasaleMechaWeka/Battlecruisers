namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters
{
    public interface IPvPSmoothZoomAdjuster
    {
        /// <returns>
		/// <c>true</c>, if the zoom level is the same as the given 
		/// <paramref name="targetOrthographicSize"/>, <c>false</c> otherwise.
		/// </returns>
		bool AdjustZoom(float targetOrthographicSize);
    }
}

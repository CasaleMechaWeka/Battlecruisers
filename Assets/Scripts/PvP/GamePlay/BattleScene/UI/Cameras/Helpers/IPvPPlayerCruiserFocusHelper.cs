namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public interface IPvPPlayerCruiserFocusHelper
    {
        /// <summary>
        /// Focuses in the player cruiser if the camera is not already
        /// roughly on the player cruiser.
        /// </summary>
        void FocusOnPlayerCruiserIfNeeded();

        /// <summary>
        /// Focuses in the player cruiser bow slot if the camera is not already
        /// roughly on the bow slot.
        /// </summary>
        void FocusOnPlayerBowSlotIfNeeded();
    }
}
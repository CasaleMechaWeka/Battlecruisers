using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters
{
    public interface IPvPSmoothPositionAdjuster
    {
        /// <returns>
        /// <c>true</c>, if the camera position is the same as the given 
        /// <paramref name="targetPosition"/>, <c>false</c> otherwise.
        /// </returns>
        bool AdjustPosition(Vector3 targetPosition);
    }
}

using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPScrollRecogniser : IPvPScrollRecogniser
    {
        /// <returns>
        /// True if the delta represents a swipe, false if the delta represents a zoom.
        /// </returns>
        public bool IsScroll(Vector2 delta)
        {
            // Weighted towards choosing vertical change (zoom)
            // over horizontal change (scroll)
            return Mathf.Abs(delta.x) >= 2 * Mathf.Abs(delta.y);
        }
    }
}
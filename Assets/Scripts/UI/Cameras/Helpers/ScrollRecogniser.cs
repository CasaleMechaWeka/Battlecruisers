using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class ScrollRecogniser : IScrollRecogniser
    {
        /// <returns>
        /// True if the delta represents a swipe, false if the delta represents a zoom.
        /// </returns>
        public bool IsScroll(Vector2 delta)
        {
            // Heavily weighted towards choosing vertical change (zoom)
            // over horizontal change (scroll)
            return Mathf.Abs(delta.x) >= 4 * Mathf.Abs(delta.y);
        }
    }
}
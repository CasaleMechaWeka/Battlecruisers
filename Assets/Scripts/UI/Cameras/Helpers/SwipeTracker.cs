using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.Cameras.Helpers
{
    // FELIX  Interface
    // FELIX  Use in SwipeCTP
    public class SwipeTracker : MonoBehaviour, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            Logging.Log(Tags.SWIPE_NAVIGATION, $"delta: {eventData.delta}");
        }
    }
}
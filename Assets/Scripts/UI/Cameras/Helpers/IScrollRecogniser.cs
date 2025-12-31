using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public interface IScrollRecogniser
    {
        bool IsScroll(Vector2 delta);
    }
}
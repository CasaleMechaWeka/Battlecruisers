using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class StaticScrollRecogniser : IScrollRecogniser
    {
        private readonly bool _isScroll;
        
        public StaticScrollRecogniser(bool isScroll)
        {
            _isScroll = isScroll;
        }

        public bool IsScroll(Vector2 delta)
        {
            return _isScroll;
        }
    }
}
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems
{
    public class LockedItem : MonoBehaviourWrapper
    {
        private RectTransform _rectTransform;

        // FELIX  Still used?
        public Vector2 Size { get { return _rectTransform.sizeDelta; } }

        public void Initialise()
        {
            _rectTransform = transform.Parse<RectTransform>();
        }
    }
}

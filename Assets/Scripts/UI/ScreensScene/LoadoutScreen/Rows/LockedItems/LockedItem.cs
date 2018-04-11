using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows.LockedItems
{
	// FELIX  Show question mark :D
    public class LockedItem : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public Vector2 Size { get { return _rectTransform.sizeDelta; } }

        public void Initialise()
        {
            _rectTransform = transform.Parse<RectTransform>();
        }
    }
}

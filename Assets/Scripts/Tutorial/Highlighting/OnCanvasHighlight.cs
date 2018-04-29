using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class OnCanvasHighlight : MonoBehaviour, IHighlight
    {
        public void Initialise(float radiusInPixels)
        {
            RectTransform rectTransform = transform.Parse<RectTransform>();
            float diameter = 2 * radiusInPixels;
            rectTransform.sizeDelta = new Vector2(diameter, diameter);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}

using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class OnCanvasHighlight : MonoBehaviour, IHighlight
    {
        public void Initialise(float radius, Vector2 position)
        {
            RectTransform rectTransform = transform.Parse<RectTransform>();
            float diameter = 2 * radius;
            rectTransform.sizeDelta = new Vector2(diameter, diameter);
        }

        void IHighlight.Destroy()
        {
            Destroy(gameObject);
        }
    }
}

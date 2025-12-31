using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Tutorial.Highlighting.FourSquare
{
    public abstract class RectangleImage : MonoBehaviour
    {
        private Image _rectangleImage;

        public void Initialise()
        {
            _rectangleImage = GetComponent<Image>();
            Assert.IsNotNull(_rectangleImage);
        }

        public void UpdatePosition(HighlightArgs args)
        {
            Vector2 highlightSize = _rectangleImage.rectTransform.sizeDelta * _rectangleImage.rectTransform.lossyScale;
            _rectangleImage.transform.position = FindPosition(args, highlightSize);
        }

        protected abstract Vector2 FindPosition(HighlightArgs args, Vector2 highlightSize);
    }
}
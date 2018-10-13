using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public abstract class MaskImage : MonoBehaviour
    {
        private Image _maskImage;

        public void Initialise()
        {
            _maskImage = GetComponent<Image>();
            Assert.IsNotNull(_maskImage);
        }

        public void UpdatePosition(Vector2 highlightPosition, Vector2 highlightSize)
        {
            _maskImage.transform.position = FindPosition(highlightPosition, highlightSize, _maskImage.rectTransform.sizeDelta);
        }

        protected abstract Vector2 FindPosition(Vector2 highlightPosition, Vector2 highlightSize, Vector2 maskSize);
    }
}
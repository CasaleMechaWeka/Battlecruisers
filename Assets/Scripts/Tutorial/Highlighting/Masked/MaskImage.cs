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

        public void UpdatePosition(HighlightArgs args)
        {
            _maskImage.transform.position = FindPosition(args, _maskImage.rectTransform.sizeDelta);
        }

        protected abstract Vector2 FindPosition(HighlightArgs args, Vector2 maskSize);
    }
}
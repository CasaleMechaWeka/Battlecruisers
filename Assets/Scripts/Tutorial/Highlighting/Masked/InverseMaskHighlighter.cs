using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class InverseMaskHighlighter : MonoBehaviour, ICoreHighlighter
    {
        private RectTransform _rectTransform;

        public GameObject modalMainMenuButton;

        public void Initialise()
        {
            Assert.IsNotNull(modalMainMenuButton);
            _rectTransform = transform.Parse<RectTransform>();
        }

        public void Highlight(HighlightArgs args)
        {
            _rectTransform.position = args.CenterPosition;
            _rectTransform.sizeDelta = args.Size / _rectTransform.lossyScale;

            gameObject.SetActive(true);
            modalMainMenuButton.SetActive(true);
        }

        public void Unhighlight()
        {
            gameObject.SetActive(false);
            modalMainMenuButton.SetActive(false);
        }
    }
}
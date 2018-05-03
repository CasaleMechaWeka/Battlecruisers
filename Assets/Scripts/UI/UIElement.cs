using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI
{
    public class UIElement : MonoBehaviour, IHighlightable
    {
        private RectTransform _rectTransform;

        public Transform Transform { get { return transform; } }
		public virtual Vector2 PositionAdjustment { get { return Vector2.zero; } }
        public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		public virtual float SizeMultiplier { get { return 1.3f; } }
        public HighlightableType Type { get { return HighlightableType.OnCanvas; } }

        public virtual void Initialise()
        {
            _rectTransform = transform.Parse<RectTransform>();
        }
    }
}

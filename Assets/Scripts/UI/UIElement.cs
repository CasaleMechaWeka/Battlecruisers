using BattleCruisers.Tutorial.Highlighting;
using UnityEngine;

namespace BattleCruisers.UI
{
    public class UIElement : Togglable, IHighlightable
    {
        public Transform Transform { get { return transform; } }
		public virtual Vector2 PositionAdjustment { get { return Vector2.zero; } }
        public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		public virtual float SizeMultiplier { get { return 1.3f; } }
        public HighlightableType HighlightableType { get { return HighlightableType.OnCanvas; } }
    }
}

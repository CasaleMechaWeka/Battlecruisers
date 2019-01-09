using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI
{
    public class UIElement : Togglable, IHighlightable
    {
        public ITransform Transform { get; private set; }
		public virtual Vector2 PositionAdjustment { get { return Vector2.zero; } }
        public Vector2 Size { get { return _rectTransform.sizeDelta; } }
		public virtual float SizeMultiplier { get { return 1.3f; } }
        public HighlightableType HighlightableType { get { return HighlightableType.OnCanvas; } }

        public override void Initialise()
        {
            base.Initialise();
            Transform = new TransformBC(transform);
        }
    }
}

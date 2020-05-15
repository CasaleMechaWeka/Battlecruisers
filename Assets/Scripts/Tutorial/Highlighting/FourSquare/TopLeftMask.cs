using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.FourSquare
{
    public class TopLeftMask : MaskImage
    {
        protected override Vector2 FindPosition(HighlightArgs args, Vector2 maskSize)
        {
            return
                new Vector2(
                    args.BottomLeftPosition.x - maskSize.x,
                    args.BottomLeftPosition.y);
        }
    }
}
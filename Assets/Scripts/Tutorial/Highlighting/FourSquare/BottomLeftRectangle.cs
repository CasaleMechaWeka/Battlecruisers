using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.FourSquare
{
    public class BottomLeftRectangle : RectangleImage
    {
        protected override Vector2 FindPosition(HighlightArgs args, Vector2 maskSize)
        {
            return
                new Vector2(
                    args.BottomLeftPosition.x + args.Size.x - maskSize.x,
                    args.BottomLeftPosition.y - maskSize.y);
        }
    }
}
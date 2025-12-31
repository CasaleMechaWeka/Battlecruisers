using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.FourSquare
{
    public class BottomRightRectangle : RectangleImage
    {
        protected override Vector2 FindPosition(HighlightArgs args, Vector2 highlightSize)
        {
            return
                new Vector2(
                    args.BottomLeftPosition.x + args.Size.x,
                    args.BottomLeftPosition.y + args.Size.y - highlightSize.y);
        }
    }
}
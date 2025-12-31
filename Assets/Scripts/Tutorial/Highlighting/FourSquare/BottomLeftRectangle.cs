using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.FourSquare
{
    public class BottomLeftRectangle : RectangleImage
    {
        protected override Vector2 FindPosition(HighlightArgs args, Vector2 highlightSize)
        {
            return
                new Vector2(
                    args.BottomLeftPosition.x + args.Size.x - highlightSize.x,
                    args.BottomLeftPosition.y - highlightSize.y);
        }
    }
}
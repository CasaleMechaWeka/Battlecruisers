using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.FourSquare
{
    public class TopLeftRectangle : RectangleImage
    {
        protected override Vector2 FindPosition(HighlightArgs args, Vector2 highlightSize)
        {
            return
                new Vector2(
                    args.BottomLeftPosition.x - highlightSize.x,
                    args.BottomLeftPosition.y);
        }
    }
}
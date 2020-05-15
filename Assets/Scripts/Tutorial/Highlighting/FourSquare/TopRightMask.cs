using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.FourSquare
{
    public class TopRightMask : MaskImage
    {
        protected override Vector2 FindPosition(HighlightArgs args, Vector2 maskSize)
        {
            return
                new Vector2(
                    args.BottomLeftPosition.x,
                    args.BottomLeftPosition.y + args.Size.y);
        }
    }
}
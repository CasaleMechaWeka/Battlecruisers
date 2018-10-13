using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class BottomRightMask : MaskImage
    {
        protected override Vector2 FindPosition(Vector2 highlightPosition, Vector2 highlightSize, Vector2 maskSize)
        {
            return
                new Vector2(
                    highlightPosition.x + highlightSize.x,
                    highlightPosition.y + highlightSize.y - maskSize.y);
        }
    }
}
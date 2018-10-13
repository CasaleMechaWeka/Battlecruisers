using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class HighlightArgs
    {
        public Vector2 BottomLeftPosition { get; private set; }
        public Vector2 Size { get; private set; }

        public HighlightArgs(Vector2 bottomLeftPosition, Vector2 size)
        {
            BottomLeftPosition = bottomLeftPosition;
            Size = size;
        }
    }
}
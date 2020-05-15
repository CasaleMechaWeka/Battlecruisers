using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class HighlightArgs
    {
        public Vector2 CenterPosition { get; }
        public Vector2 BottomLeftPosition { get; }
        public Vector2 Size { get; }

        public HighlightArgs(Vector2 centerPosition, Vector2 bottomLeftPosition, Vector2 size)
        {
            CenterPosition = centerPosition;
            BottomLeftPosition = bottomLeftPosition;
            Size = size;
        }
    }
}
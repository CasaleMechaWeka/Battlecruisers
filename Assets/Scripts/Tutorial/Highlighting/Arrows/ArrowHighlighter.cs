using BattleCruisers.Tutorial.Highlighting.Masked;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Arrows
{
    // FELIX  Only highlight small objects (eg:  Don't need to point arrow at cruiser :P)
    // FELIX  Don't make MonoBehaviour?
    // FELIX  Abstract into multiple classes :P
    // FELIX  Test
    public class ArrowHighlighter : MonoBehaviour, IMaskHighlighter
    {
        public void Highlight(HighlightArgs args)
        {
            gameObject.SetActive(true);

            //// Bottom left
            //gameObject.transform.position = args.BottomLeftPosition;
            //gameObject.transform.up = FindArrowDirection(args.BottomLeftPosition, args.CenterPosition);

            // Bottom right
            Vector2 bottomRight
                = new Vector2(
                    args.CenterPosition.x + args.Size.x / 2,
                    args.CenterPosition.y - args.Size.y / 2);
            gameObject.transform.position = bottomRight;
            gameObject.transform.up = FindArrowDirection(bottomRight, args.CenterPosition);
        }

        private Vector2 FindArrowDirection(Vector2 corner, Vector2 center)
        {
            return center - corner;
        }

        public void Unhighlight()
        {
            gameObject.SetActive(false);
        }
    }
}
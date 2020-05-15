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

            gameObject.transform.position = args.BottomLeftPosition;
            gameObject.transform.up = FindArrowDirection(args.BottomLeftPosition, args.CenterPosition);
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
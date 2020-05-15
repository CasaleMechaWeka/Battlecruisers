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

            gameObject.transform.position = args.CenterPosition;
        }

        public void Unhighlight()
        {
            gameObject.SetActive(false);
        }
    }
}
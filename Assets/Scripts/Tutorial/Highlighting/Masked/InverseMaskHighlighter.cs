using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class InverseMaskHighlighter : MonoBehaviour, IMaskHighlighter
    {
        public void Highlight(HighlightArgs args)
        {
            // FELIX  Position :P

            gameObject.SetActive(true);
        }

        public void Unhighlight()
        {
            gameObject.SetActive(false);
        }
    }
}
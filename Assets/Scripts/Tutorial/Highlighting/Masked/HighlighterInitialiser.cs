using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class HighlighterInitialiser : MonoBehaviour
    {
        public InverseMaskHighlighter inverseHighlighter;
        public MaskHighlighter maskHighlighter;

        public IMaskHighlighter CreateHighlighter()
        {
            Helper.AssertIsNotNull(inverseHighlighter, maskHighlighter);
            return new DualMaskHighlighter(inverseHighlighter, maskHighlighter);
        }
    }
}
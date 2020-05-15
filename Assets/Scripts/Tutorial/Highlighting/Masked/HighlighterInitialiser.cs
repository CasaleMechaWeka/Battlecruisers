using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    /// <summary>
    /// Uses two types of masking:
    /// 1. UI inverse masking to show highlight to user
    /// 2. Original manual masking (placing 4 rectangles around highlight point), 
    ///     to block raycasts (user input) to anything outside of the highlight zone.
    /// </summary>
    public class HighlighterInitialiser : MonoBehaviour
    {
        public InverseMaskHighlighter inverseHighlighter;
        public MaskHighlighter maskHighlighter;

        public IMaskHighlighter CreateHighlighter()
        {
            Helper.AssertIsNotNull(inverseHighlighter, maskHighlighter);

            inverseHighlighter.Initialise();
            maskHighlighter.Initialise();

            return new CompositeHighlighter(inverseHighlighter, maskHighlighter);
        }
    }
}
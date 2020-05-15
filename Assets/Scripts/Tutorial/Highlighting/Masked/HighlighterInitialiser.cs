using BattleCruisers.Tutorial.Highlighting.Arrows;
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
    /// FELIX  Improve highlight naming and namespaces :)
    public class HighlighterInitialiser : MonoBehaviour
    {
        public InverseMaskHighlighter inverseHighlighter;
        public MaskHighlighter maskHighlighter;
        public ArrowHighlighter arrowHighlighter;

        public IMaskHighlighter CreateHighlighter()
        {
            Helper.AssertIsNotNull(inverseHighlighter, maskHighlighter, arrowHighlighter);

            inverseHighlighter.Initialise();
            maskHighlighter.Initialise();

            return new CompositeHighlighter(inverseHighlighter, maskHighlighter, arrowHighlighter);
        }
    }
}
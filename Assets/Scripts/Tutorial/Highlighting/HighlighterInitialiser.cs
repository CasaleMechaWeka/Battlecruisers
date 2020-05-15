using BattleCruisers.Tutorial.Highlighting.Arrows;
using BattleCruisers.Tutorial.Highlighting.FourSquare;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    /// <summary>
    /// FELIX  Update comments :P
    /// Uses two types of masking:
    /// 1. UI inverse masking to show highlight to user
    /// 2. Original manual masking (placing 4 rectangles around highlight point), 
    ///     to block raycasts (user input) to anything outside of the highlight zone.
    /// </summary>
    /// FELIX  Improve highlight naming and namespaces :)
    public class HighlighterInitialiser : MonoBehaviour
    {
        public InverseMaskHighlighter inverseHighlighter;
        public FourSquareHighlighter fourSquareHighlighter;
        public ArrowHighlighter arrowHighlighter;

        public IMaskHighlighter CreateHighlighter(ICamera camera)
        {
            Assert.IsNotNull(camera);
            Helper.AssertIsNotNull(inverseHighlighter, fourSquareHighlighter, arrowHighlighter);

            inverseHighlighter.Initialise();
            fourSquareHighlighter.Initialise();

            arrowHighlighter
                .Initialise(
                     new ArrowCalculator(camera));

            return new CompositeHighlighter(inverseHighlighter, fourSquareHighlighter, arrowHighlighter);
        }
    }
}
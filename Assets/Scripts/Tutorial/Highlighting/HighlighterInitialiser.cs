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
    /// Uses 3 highlighters:
    /// 1. InverseMaskHighlighter:  Covers everything except what's being highlighted
    /// 2. FourSquareHighlighter:  Blocks raycasts so non highlighted objects are not clickabl.
    /// 3. ArrowHighlighter:  Points an arrow at the object being highlighted
    /// </summary>
    public class HighlighterInitialiser : MonoBehaviour
    {
        public InverseMaskHighlighter inverseHighlighter;
        public FourSquareHighlighter fourSquareHighlighter;
        public ArrowHighlighter arrowHighlighter;

        public ICoreHighlighter CreateHighlighter(ICamera camera)
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
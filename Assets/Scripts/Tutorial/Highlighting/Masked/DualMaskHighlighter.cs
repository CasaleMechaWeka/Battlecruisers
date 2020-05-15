using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    /// <summary>
    /// Uses two types of masking:
    /// 1. UI inverse masking to show highlight to user
    /// 2. Original manual masking (placing 4 rectangles around highlight point), 
    ///     to block raycasts (user input) to anything outside of the highlight zone.
    /// </summary>
    public class DualMaskHighlighter : IMaskHighlighter
    {
        private readonly IMaskHighlighter _mainHighlighter, _raycastBlocker;

        public DualMaskHighlighter(IMaskHighlighter mainHighlighter, IMaskHighlighter raycastBlocker)
        {
            Helper.AssertIsNotNull(mainHighlighter, raycastBlocker);

            _mainHighlighter = mainHighlighter;
            _raycastBlocker = raycastBlocker;
        }

        public void Highlight(HighlightArgs args)
        {
            _mainHighlighter.Highlight(args);
            _raycastBlocker.Highlight(args);
        }

        public void Unhighlight()
        {
            _mainHighlighter.Unhighlight();
            _raycastBlocker.Unhighlight();
        }
    }
}
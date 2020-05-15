using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.FourSquare;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Tutorial
{
    public class FourSquareHighlighterTestGod : MovingHighlightableTestGod
    {
        protected override ICoreHighlighter CreateHighlighter(ICamera camera)
        {
            FourSquareHighlighter highlighter = GetComponentInChildren<FourSquareHighlighter>();
            Assert.IsNotNull(highlighter);
            highlighter.Initialise();
            return highlighter;
        }
    }
}
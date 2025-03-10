using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Tutorial
{
    public class InverseMaskHighlighterTestGod : MovingHighlightableTestGod
    {
        protected override ICoreHighlighter CreateHighlighter(ICamera camera)
        {
            InverseMaskHighlighter highlighter = GetComponentInChildren<InverseMaskHighlighter>();
            Assert.IsNotNull(highlighter);
            highlighter.Initialise();
            return highlighter;
        }
    }
}
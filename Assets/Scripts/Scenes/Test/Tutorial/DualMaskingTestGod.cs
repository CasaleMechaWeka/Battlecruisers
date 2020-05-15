using BattleCruisers.Tutorial.Highlighting.Masked;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Tutorial
{
    public class DualMaskingTestGod : MovingUIMaskTestGod
    {
        protected override IMaskHighlighter CreateHighlighter()
        {
            HighlighterInitialiser highlighterInitialiser = GetComponent<HighlighterInitialiser>();
            Assert.IsNotNull(highlighterInitialiser);
            return highlighterInitialiser.CreateHighlighter();
        }
    }
}
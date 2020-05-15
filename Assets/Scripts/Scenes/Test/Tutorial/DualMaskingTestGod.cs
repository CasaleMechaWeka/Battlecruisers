using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Tutorial
{
    public class DualMaskingTestGod : MovingUIMaskTestGod
    {
        protected override IMaskHighlighter CreateHighlighter(ICamera camera)
        {
            HighlighterInitialiser highlighterInitialiser = GetComponentInChildren<HighlighterInitialiser>();
            Assert.IsNotNull(highlighterInitialiser);
            return highlighterInitialiser.CreateHighlighter(camera);
        }
    }
}
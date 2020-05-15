using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Tutorial
{
    public class CompositeHighlightTestGod : MovingUIMaskTestGod
    {
        protected override ICoreHighlighter CreateHighlighter(ICamera camera)
        {
            HighlighterInitialiser highlighterInitialiser = GetComponentInChildren<HighlighterInitialiser>();
            Assert.IsNotNull(highlighterInitialiser);
            return highlighterInitialiser.CreateHighlighter(camera);
        }
    }
}
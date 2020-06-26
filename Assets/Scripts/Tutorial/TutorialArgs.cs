using BattleCruisers.Tutorial.Explanation;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgs : TutorialArgsBase, ITutorialArgs
    {
        public ExplanationPanel ExplanationPanel { get; }

        public TutorialArgs(ITutorialArgsBase baseArgs, ExplanationPanel explanationPanel)
            : base(baseArgs)
        {
            Assert.IsNotNull(explanationPanel);
            ExplanationPanel = explanationPanel;
        }
    }
}

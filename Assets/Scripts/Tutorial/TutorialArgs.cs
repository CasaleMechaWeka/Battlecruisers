using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.UI;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgs : TutorialArgsBase, ITutorialArgs
    {
        public ExplanationPanel ExplanationPanel { get; }
        public IButton ModalMainMenuButton { get; }

        public TutorialArgs(
            ITutorialArgsBase baseArgs, 
            ExplanationPanel explanationPanel,
            IButton modalMainMenuButton)
            : base(baseArgs)
        {
            Helper.AssertIsNotNull(explanationPanel, modalMainMenuButton);

            ExplanationPanel = explanationPanel;
            ModalMainMenuButton = modalMainMenuButton;
        }
    }
}

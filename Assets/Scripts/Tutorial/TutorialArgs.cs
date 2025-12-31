using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.UI;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgs : TutorialArgsBase
    {
        public ExplanationPanel ExplanationPanel { get; }
        public IButton ModalMainMenuButton { get; }

        public TutorialArgs(
            TutorialArgsBase baseArgs,
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

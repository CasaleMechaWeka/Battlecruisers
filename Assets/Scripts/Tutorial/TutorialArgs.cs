using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgs : TutorialArgsBase, ITutorialArgs
    {
        public ExplanationPanel ExplanationPanel { get; }
        public IButton ModalMainMenuButton { get; }
        public ILocTable TutorialStrings { get; }

        public TutorialArgs(
            ITutorialArgsBase baseArgs, 
            ExplanationPanel explanationPanel,
            IButton modalMainMenuButton,
            ILocTable tutorialStrings)
            : base(baseArgs)
        {
            Helper.AssertIsNotNull(explanationPanel, modalMainMenuButton, tutorialStrings);

            ExplanationPanel = explanationPanel;
            ModalMainMenuButton = modalMainMenuButton;
            TutorialStrings = tutorialStrings;
        }
    }
}

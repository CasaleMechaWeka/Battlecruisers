using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class MainMenuStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IButton _modalMainMenuButton;
        private readonly IModalMenu _mainMenu;
        // FELIX  Remove :)
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public MainMenuStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IButton modalMainMenuButton,
            IModalMenu mainMenu,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(mainMenu, modalMainMenuButton, explanationDismissableStepFactory);

            _modalMainMenuButton = modalMainMenuButton;
            _mainMenu = mainMenu;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Get user to open main menu
            ITutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    "This is the main menu button.  Open the menu :D",
                    _modalMainMenuButton);
            steps.Add(
                new ExplanationClickStep(
                    args,
                    _modalMainMenuButton));

            // Wait for main menu to be dismissed
            steps.Add(
                new MenuDismissedWaitStep(
                    _argsFactory.CreateTutorialStepArgs(),
                    _mainMenu));

            return steps;
        }
    }
}
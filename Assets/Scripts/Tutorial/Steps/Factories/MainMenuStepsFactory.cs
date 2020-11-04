using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class MainMenuStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IButton _modalMainMenuButton;
        // FELIX  Remove :)
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public MainMenuStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IButton modalMainMenuButton,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(modalMainMenuButton, explanationDismissableStepFactory);

            _modalMainMenuButton = modalMainMenuButton;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            ITutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    "This is the main menu button.  Open the menu :D",
                    _modalMainMenuButton);
            steps.Add(
                new ExplanationClickStep(
                    args,
                    _modalMainMenuButton));

            return steps;
        }
    }
}
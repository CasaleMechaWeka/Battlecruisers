using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.UI;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class MainMenuStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IButton _mainMenuButton;
        // FELIX  Remove :)
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public MainMenuStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            IButton mainMenuButton,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(mainMenuButton, explanationDismissableStepFactory);

            _mainMenuButton = mainMenuButton;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            ITutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    "This is the main menu button",
                    _mainMenuButton);
            steps.Add(
                new ExplanationClickStep(
                    args,
                    _mainMenuButton));

            return steps;
        }
    }
}
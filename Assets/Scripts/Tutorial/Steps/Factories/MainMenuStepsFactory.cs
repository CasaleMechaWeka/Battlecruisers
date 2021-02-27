using BattleCruisers.Tutorial.Steps.ClickSteps;
using BattleCruisers.Tutorial.Steps.WaitSteps;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class MainMenuStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IButton _modalMainMenuButton;
        private readonly IModalMenu _mainMenu;

        public MainMenuStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ILocTable tutorialStrings,
            IButton modalMainMenuButton,
            IModalMenu mainMenu) 
            : base(argsFactory, tutorialStrings)
        {
            Helper.AssertIsNotNull(mainMenu, modalMainMenuButton);

            _modalMainMenuButton = modalMainMenuButton;
            _mainMenu = mainMenu;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // FELIX  Loc
            // Get user to open main menu
            ITutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    "This is the Main Menu.",
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
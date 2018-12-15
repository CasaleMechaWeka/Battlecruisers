using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class EnemyCruiserStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IAutoNavigationStepFactory _autNavigationStepFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public EnemyCruiserStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ITutorialArgs tutorialArgs,
            IAutoNavigationStepFactory autoNavigationStepFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory, tutorialArgs)
        {
            Helper.AssertIsNotNull(autoNavigationStepFactory, explanationDismissableStepFactory);

            _autNavigationStepFactory = autoNavigationStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateTutorialSteps()
        {
            List<ITutorialStep> steps = new List<ITutorialStep>();

            steps.AddRange(_autNavigationStepFactory.CreateSteps(CameraFocuserTarget.AICruiser));

            ITutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    "This is the enemy cruiser.  You win if you destroy their cruiser before it destroys you.",
                    _tutorialArgs.AICruiser);

            steps.Add(_explanationDismissableStepFactory.CreateTutorialStep(args));

            return steps;
        }
    }
}
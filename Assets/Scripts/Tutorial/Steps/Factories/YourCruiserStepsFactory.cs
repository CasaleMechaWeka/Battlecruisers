using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class YourCruiserStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly ICruiser _playerCruiser;
        private readonly ITutorialStepFactory _cameraAdjustmentWaitStepFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;

        public YourCruiserStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ITutorialArgs tutorialArgs,
            ICruiser playerCruiser,
            ITutorialStepFactory cameraAdjustmentWaitStepFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory) 
            : base(argsFactory, tutorialArgs)
        {
            Helper.AssertIsNotNull(playerCruiser, cameraAdjustmentWaitStepFactory, explanationDismissableStepFactory);

            _playerCruiser = playerCruiser;
            _cameraAdjustmentWaitStepFactory = cameraAdjustmentWaitStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
        }

        public IList<ITutorialStep> CreateTutorialSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            steps.Add(_cameraAdjustmentWaitStepFactory.CreateTutorialStep());

            ITutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    "This is your awesome cruiser :D",
                    _playerCruiser);

            steps.Add(_explanationDismissableStepFactory.CreateTutorialStep(args));

            return steps;
        }
    }
}
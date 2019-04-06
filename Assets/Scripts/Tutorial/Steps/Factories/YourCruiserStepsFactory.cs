using BattleCruisers.Cruisers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class YourCruiserStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly ICruiser _playerCruiser;
        private readonly ITutorialStepFactory _cameraAdjustmentWaitStepFactory;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _navigationPermitter;

        public YourCruiserStepsFactory(
            ITutorialStepArgsFactory argsFactory,
            ICruiser playerCruiser,
            ITutorialStepFactory cameraAdjustmentWaitStepFactory,
            IExplanationDismissableStepFactory explanationDismissableStepFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter navigationPermitter) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(playerCruiser, cameraAdjustmentWaitStepFactory, explanationDismissableStepFactory, featurePermitterStepFactory, navigationPermitter);

            _playerCruiser = playerCruiser;
            _cameraAdjustmentWaitStepFactory = cameraAdjustmentWaitStepFactory;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _featurePermitterStepFactory = featurePermitterStepFactory;
            _navigationPermitter = navigationPermitter;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationPermitter, enableFeature: false));
            steps.Add(_cameraAdjustmentWaitStepFactory.CreateStep());

            ITutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    "This is your awesome cruiser :D",
                    _playerCruiser);

            steps.Add(_explanationDismissableStepFactory.CreateStep(args));

            return steps;
        }
    }
}
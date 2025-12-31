using BattleCruisers.Cruisers;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public class YourCruiserStepsFactory : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly ICruiser _playerCruiser;
        private readonly CameraAdjustmentWaitStepFactory _cameraAdjustmentWaitStepFactory;
        private readonly ExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly FeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _navigationPermitter;

        public YourCruiserStepsFactory(
            TutorialStepArgsFactory argsFactory,
            ICruiser playerCruiser,
            CameraAdjustmentWaitStepFactory cameraAdjustmentWaitStepFactory,
            ExplanationDismissableStepFactory explanationDismissableStepFactory,
            FeaturePermitterStepFactory featurePermitterStepFactory,
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

        public IList<TutorialStep> CreateSteps()
        {
            IList<TutorialStep> steps = new List<TutorialStep>();

            steps.Add(_featurePermitterStepFactory.CreateStep(_navigationPermitter, enableFeature: false));
            steps.Add(_cameraAdjustmentWaitStepFactory.CreateStep());

            TutorialStepArgs args
                = _argsFactory.CreateTutorialStepArgs(
                    LocTableCache.TutorialTable.GetString("Steps/YourCruiser"),
                    _playerCruiser);

            steps.Add(_explanationDismissableStepFactory.CreateStep(args));

            return steps;
        }
    }
}
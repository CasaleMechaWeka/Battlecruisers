using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Tutorial.Steps.Factories
{
    public abstract class SwipeStepsFactoryBase : TutorialFactoryBase, ITutorialStepsFactory
    {
        private readonly IFeaturePermitterStepFactory _featurePermitterStepFactory;
        private readonly IPermitter _swipePermitter;
        private readonly IExplanationDismissableStepFactory _explanationDismissableStepFactory;
        private readonly string _message;

        protected SwipeStepsFactoryBase(
            ITutorialStepArgsFactory argsFactory,
            IFeaturePermitterStepFactory featurePermitterStepFactory,
            IPermitter swipePermitter,
            IExplanationDismissableStepFactory explanationDismissableStepFactory,
            string message) 
            : base(argsFactory)
        {
            Helper.AssertIsNotNull(featurePermitterStepFactory, swipePermitter, explanationDismissableStepFactory);

            _featurePermitterStepFactory = featurePermitterStepFactory;
            _swipePermitter = swipePermitter;
            _explanationDismissableStepFactory = explanationDismissableStepFactory;
            _message = message;
        }

        public IList<ITutorialStep> CreateSteps()
        {
            IList<ITutorialStep> steps = new List<ITutorialStep>();

            // Enable swiping
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: true));

            // Explain swiping, encourage user to experiment
            steps.Add(
                _explanationDismissableStepFactory.CreateStepWithSecondaryButton(
                    _argsFactory.CreateTutorialStepArgs(_message)));

            // Disable swiping
            steps.Add(_featurePermitterStepFactory.CreateStep(_swipePermitter, enableFeature: false));

            return steps;
        }
    }
}